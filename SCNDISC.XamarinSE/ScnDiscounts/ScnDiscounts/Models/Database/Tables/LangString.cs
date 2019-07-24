using SQLite;

namespace ScnDiscounts.Models.Database.Tables
{
    public class LangString
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }
        [Indexed]
        public string LanguageCode { get; set; }
        public string Text { get; set; }
    }
}
