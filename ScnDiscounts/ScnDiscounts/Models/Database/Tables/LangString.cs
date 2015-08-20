using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScnDiscounts.Models.Database.Tables
{
    public class LangString
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string LanguageCode { get; set; }
        public string Text { get; set; }
    }
}
