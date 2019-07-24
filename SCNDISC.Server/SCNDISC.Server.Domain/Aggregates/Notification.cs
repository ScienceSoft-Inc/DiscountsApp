using System;

namespace SCNDISC.Server.Domain.Aggregates
{
    public class Notification : Aggregate
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string DocumentId { get; set; }
        public DateTime Created { get; set; }
        public bool IsSentToAllDevices { get; set; }
        public string Language { get; set; }
    }
}
