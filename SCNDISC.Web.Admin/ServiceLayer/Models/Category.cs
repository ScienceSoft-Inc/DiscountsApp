namespace SCNDISC.Web.Admin.ServiceLayer
{
    public class Category
    {
        public string Id { get; set; }
        public string Name_RU { set; get; }
        public string Name_EN { set; get; }
        public string Color { get; set; }

        public string Names =>
            string.Format(
                "<input type='hidden' value='{2}' /><span class='x-label-ru'>{0}</span><span class='x-label-en'>{1}</span>",
                Name_RU, Name_EN, string.IsNullOrEmpty(Color) ? "transparent" : Color);
    }
}