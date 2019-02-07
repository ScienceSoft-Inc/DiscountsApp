using System;

namespace SCNDISC.Server.Domain.Aggregates
{
	public class Feedback : Aggregate
	{
		public string UserName { get; set; }
		public string Message { get; set; }
		public DateTime Created { get; set; }
	}
}