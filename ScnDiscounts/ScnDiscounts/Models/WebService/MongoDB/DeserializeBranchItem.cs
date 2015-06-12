using System.Collections.Generic;

namespace ScnDiscounts.Models.WebService.MongoDB
{
    public class DeserializeBranchItem
    {
        public List<DeserializeLocText> Description { get; set; }
        public List<DeserializeLocText> Address { get; set; }
        public string PartnerId { get; set; }
        public DeserializeLocation Location { get; set; }
        public string Timetable { get; set; }
        public List<DeserializeDiscount> Discounts { get; set; }
        public List<DeserializeCategorie> Categories { get; set; }
        public List<DeserializePhone> Phones { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Image { get; set; }

        public string Id { get; set; }
        public List<DeserializeLocText> Name { get; set; }
    }

    public class DeserializeLocText
    {
        public string Lan { get; set; }
        public string LocText { get; set; }
    }

    public class DeserializeDiscount
    {
        public List<DeserializeLocText> Name { get; set; }
    }

    public class DeserializeCategorie
    {
        public string Type { get; set; }
        public List<DeserializeLocText> Name { get; set; }
    }

    public class DeserializePhone
    {
        public string Number { get; set; }
    }

    public class DeserializeLocation
    {
        public DeserializeCoordinates Coordinates { get; set; }
        public string Type { get; set; }
    }

    public class DeserializeCoordinates
    {
        public List<string>  Values { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
