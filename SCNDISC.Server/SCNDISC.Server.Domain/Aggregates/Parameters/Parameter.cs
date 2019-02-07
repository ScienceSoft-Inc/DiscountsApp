namespace SCNDISC.Server.Domain.Aggregates.Parameters
{
    public class Parameter: Aggregate
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
