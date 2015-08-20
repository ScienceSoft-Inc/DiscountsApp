using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScnDiscounts.Models.Database.Tables
{
    public class Categorie
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int DiscountId { get; set; }
        public int TypeCode { get; set; }
    }
}
