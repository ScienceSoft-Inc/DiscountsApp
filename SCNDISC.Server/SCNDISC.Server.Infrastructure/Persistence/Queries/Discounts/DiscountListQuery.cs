using MongoDB.Driver;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Queries.Discounts;
using SCNDISC.Server.Infrastructure.Persistence.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCNDISC.Server.Infrastructure.Persistence.Queries.Discounts
{
    public class DiscountListQuery : MongoCommandQueryBase<Branch>, IDiscountListQuery
    {
        public DiscountListQuery(IMongoCollectionProvider collectionProvider) : base(collectionProvider) { }

        private class PartnerOnly
        {
            public string PartnerId;
            public IEnumerable<LocalizableText> Name;
            public IEnumerable<LocalizableText> Address;
            public IEnumerable<LocalizableText> Description;
            public string Icon;
            public IEnumerable<Phone> Phones;
            public string Timetable;
            public IEnumerable<string> Categories;
            public string Url;
            public bool IsDeleted;
            public DateTime? Modified;
            public IEnumerable<WebAddress> WebAddresses;
            public IEnumerable<Discount> Discounts;
        }

        public async Task<IEnumerable<Branch>> GetPartnersDiscountListAsync(DateTime? syncdate = null)
        {
            // TODO starting with mongoDb server 3.6+ versions we should use expr operator to get data in a one requst instead of two as below
            // var exprFilter = new BsonDocument("$expr", new BsonDocument("$eq", new BsonArray(new[] { "$_id", "$PartnerId" })));
            var partnerIds = await Collection.Aggregate().Group(x => x.PartnerId, g => new { g.Key }).ToListAsync();
            var ids = partnerIds.Select(p => p.Key).ToList();

            var filterByIds = new FilterDefinitionBuilder<Branch>().In(x => x.Id, ids);

            var partnnersOnly = Collection.
                Aggregate().
                Match(filterByIds).
                Project(g => new PartnerOnly
                {
                    PartnerId = g.PartnerId,
                    Name = g.Name,
                    Address = g.Address,
                    Description = g.Description,
                    Icon = g.Icon,
                    Phones = g.Phones,
                    Timetable = g.Timetable,
                    Categories = g.CategoryIds,
                    Url = g.Url,
                    IsDeleted = g.IsDeleted,
                    Modified = g.Modified,
                    WebAddresses = g.WebAddresses,
                    Discounts = g.Discounts
                });

            IEnumerable<PartnerOnly> result;

            if (syncdate != null)
            {
                syncdate = syncdate.Value.Kind == DateTimeKind.Utc ? syncdate : syncdate.Value.ToUniversalTime();
                result = await partnnersOnly.Match(i => i.Modified > syncdate).ToListAsync();
            }
            else
            {
                result = await partnnersOnly.ToListAsync();
            }

            return result.Select(g => new Branch
            {
                Id = g.PartnerId,
                Name = g.Name,
                Description = g.Description,
                Address = g.Address,
                Icon = g.Icon,
                Phones = g.Phones,
                Timetable = g.Timetable,
                CategoryIds = g.Categories,
                Url = g.Url,
                IsDeleted = g.IsDeleted,
                Modified = g.Modified,
                Discounts = g.Discounts,
                WebAddresses = g.WebAddresses
            }).ToList();
        }
    }
}
