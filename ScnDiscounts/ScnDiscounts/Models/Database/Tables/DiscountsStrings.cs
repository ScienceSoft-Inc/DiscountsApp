using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScnDiscounts.Models.Database.Tables
{
    public class DiscountsStrings
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public byte Appointment { get; set; }
        public int OwnerId { get; set; }
        public int LangStringId { get; set; }
    }
}
