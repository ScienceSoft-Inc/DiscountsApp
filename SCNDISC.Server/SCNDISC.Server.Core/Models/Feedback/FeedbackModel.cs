using System;
using Newtonsoft.Json;

namespace SCNDISC.Server.Core.Models.Feedback
{
	public class FeedbackModel
	{
	    [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

	    [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

	    [JsonProperty(PropertyName = "created")]
        public DateTime Created { get; set; }
	}
}