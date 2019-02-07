using System;

namespace SCNDISC.Server.WebAPI.Models.Feedback
{
	public class FeedbackModel
	{
		public string Name { get; set; }
		public string Message { get; set; }
		public DateTime Created { get; set; }
	}
}