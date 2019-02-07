using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SCNDISC.Server.Application.Services.Feedback;
using SCNDISC.Server.Domain.Aggregates;
using SCNDISC.Server.Domain.Queries.Feedbacks;
using SCNDISC.Server.WebAPI.Models;
using SCNDISC.Server.WebAPI.Models.Feedback;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SCNDISC.Server.WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
	public class FeedbackController : ApiController
	{
		private readonly IFeedbackService _feedbackService;

		public FeedbackController(IFeedbackService feedbackService)
		{
			_feedbackService = feedbackService;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="draw">Draw counter. 
		/// This is used by DataTables to ensure that the Ajax returns from 
		/// server-side processing requests are drawn in sequence by DataTables 
		/// </param>
		/// <param name="skip">Paging first record indicator. 
		/// This is the skip point in the current data set 
		/// (0 index based - i.e. 0 is the first record)
		///  </param>
		/// <param name="take">Number of records that the table can display in the current draw. 
		/// It is expected that the number of records returned will be equal to this number, unless the 
		/// server has fewer records to return. Note that this can be -1 to indicate that 
		/// all records should be returned (although that negates any benefits of 
		/// </param>
		/// <returns></returns>
		[HttpGet]
		[Route("feedbacks")]
		public async Task<IHttpActionResult> GetAsync([FromUri]int draw, [FromUri(Name = "start")]int skip, [FromUri(Name = "length")]int take)
		{
			var feedbacks = (await _feedbackService.GetAll(new FilterModel { Skip = skip, Take = take })).
				Select(x => new FeedbackModel
				{
					Name = x.UserName,
					Message = x.Message,
					Created = x.Created
				}).ToList();

			var count = await _feedbackService.GetCount();

			var result = new DateTableResult<FeedbackModel>
			{
				Draw = draw,
				Total = count,
				Filtered = count,
				Data = feedbacks
			};

            return Ok(result);
		}

        [HttpPost]
        [Route("feedbacks")]
        public async Task<IHttpActionResult> PostAsync([FromBody]FeedbackModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Name) || string.IsNullOrWhiteSpace(model.Message))
                return BadRequest("feedback invalid");

            var userName = model.Name.Trim();
            if (userName.Length > 30)
                userName = userName.Remove(30) + "...";

            var message = model.Message.Trim();
            if (message.Length > 1000)
                message = message.Remove(1000) + "...";

            var feedback = await _feedbackService.AddAsync(new Feedback
            {
                UserName = userName,
                Message = message,
                Created = DateTime.UtcNow
            });

            var isSuccess = !string.IsNullOrEmpty(feedback.Id);
            if (isSuccess)
                await SendNotificationAsync(feedback);

            return isSuccess ? (IHttpActionResult) Ok() : BadRequest("feedback not saved");
        }

	    private static async Task SendNotificationAsync(Feedback feedback)
	    {
	        var from = ConfigurationManager.AppSettings["MailFrom"];
	        var to = ConfigurationManager.AppSettings["MailTo"];

	        if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
	            return;

            var message = new MailMessage(from, to)
	        {
	            Subject = ConfigurationManager.AppSettings["MailSubject"],
	            Body = $"From {feedback.UserName}.\r\n\r\n{feedback.Message}"
	        };

            using (message)
            using (var smtpClient = new SmtpClient())
	        {
	            try
	            {
	                await smtpClient.SendMailAsync(message);
	            }
	            catch (Exception ex)
	            {
	                Debug.WriteLine(ex);
	            }
            }
	    }
	}
}