namespace SCNDISC.Server.Domain.Aggregates.Partners
{
    public class LocalizableText
    {
        public LocalizableText() { 
        }

        public LocalizableText(string lan, string locText)
        {
            Lan = lan;
            LocText = LocText;
        }

        public string Lan { get; set; }
        public string LocText { get; set; }
    }
}
