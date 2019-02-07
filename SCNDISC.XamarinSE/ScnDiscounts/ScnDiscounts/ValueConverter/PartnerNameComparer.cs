using ScnDiscounts.Helpers;
using System;
using System.Collections.Generic;

namespace ScnDiscounts.ValueConverter
{
    public class PartnerNameComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            var value1 = x.NormalizePartnerName();
            var value2 = y.NormalizePartnerName();

            return string.Compare(value1, value2, StringComparison.OrdinalIgnoreCase);
        }
    }
}
