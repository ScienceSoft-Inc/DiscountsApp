using Refit;
using System;

namespace ScnDiscounts.Models.WebService
{
    public class SyncParams
    {
        [AliasAs("syncDate")]
        public DateTime? SyncDate { get; set; }

        public SyncParams()
        {
        }

        public SyncParams(DateTime? syncDate)
            : this()
        {
            SyncDate = syncDate;
        }
    }
}
