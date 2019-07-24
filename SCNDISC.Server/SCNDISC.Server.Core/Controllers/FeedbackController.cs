using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SCNDISC.Server.Application.Services.Feedback;
using SCNDISC.Server.Core.Models;
using SCNDISC.Server.Core.Models.Feedback;
using SCNDISC.Server.Domain.Aggregates;
using SCNDISC.Server.Domain.Queries.Feedbacks;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SCNDISC.Server.Core.Controllers
{
    //move AllowAll to const
    [EnableCors("AllowAll")]
	public class FeedbackController : ControllerBase
	{
		private readonly IFeedbackService _feedbackService;
	    private readonly IConfiguration _configuration;

        public FeedbackController(IFeedbackService feedbackService, IConfiguration configuration)
		{
			_feedbackService = feedbackService;
		    _configuration = configuration;
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
		public async Task<ActionResult<object>> GetAsync([FromQuery]int draw, [FromQuery(Name = "start")]int skip, [FromQuery(Name = "length")]int take)
		{
			var feedbacks = (await _feedbackService.GetAll(new FilterModel { Skip = skip, Take = take })).
				Select(x => new FeedbackModel
				{
					Name = x.UserName,
					Message = x.Message,
					Created = x.Created
				}).Reverse().ToList();
            
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
        public async Task<ActionResult> PostAsync([FromBody]FeedbackModel model)
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
                await SendNotificationAsync(feedback, _configuration);

            return isSuccess ? (ActionResult) Ok() : BadRequest("feedback not saved");
        }

        private static async Task SendNotificationAsync(Feedback feedback, IConfiguration configuration)
        {
            var from = configuration.GetValue<string>("MailFrom");
            var to = configuration.GetValue<string>("MailTo");

            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
                return;

            var message = new MailMessage(from, to)
            {
                Subject = configuration.GetValue<string>("MailSubject"),
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