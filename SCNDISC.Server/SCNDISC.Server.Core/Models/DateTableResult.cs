using System.Collections.Generic;
using Newtonsoft.Json;

namespace SCNDISC.Server.Core.Models
{
	public class DateTableResult<TItem>
	{
		[JsonProperty(PropertyName = "draw")]
		public int Draw { get; set; }

		[JsonProperty(PropertyName = "recordsTotal")]
		public long Total { get; set; }
		
		[JsonProperty(PropertyName = "recordsFiltered")]
		public long Filtered { get; set; }

		[JsonProperty(PropertyName = "data")]
		public List<TItem> Data { get; set; }
	}
}