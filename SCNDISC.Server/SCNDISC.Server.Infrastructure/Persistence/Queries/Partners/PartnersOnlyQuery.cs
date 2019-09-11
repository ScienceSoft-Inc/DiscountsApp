using MongoDB.Driver;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Queries.Partners;
using SCNDISC.Server.Infrastructure.Persistence.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCNDISC.Server.Infrastructure.Persistence.Queries.Partners
{
    public class PartnersOnlyQuery : MongoCommandQueryBase<Branch>, IPartnersOnlyQuery
    {
        public PartnersOnlyQuery(IMongoCollectionProvider provider)
            : base(provider) { }

        public async Task<IEnumerable<Branch>> Run(string[] selectedCategories = null, string partnerName = null)
        {
            var filter = new FilterDefinitionBuilder<Branch>().Where(x => !x.IsDeleted);

            if (selectedCategories != null)
            {
                var byCategoriesFilter = new FilterDefinitionBuilder<Branch>().Where(x =>
                    x.CategoryIds == null || !x.CategoryIds.Any() || x.CategoryIds.Any(i => selectedCategories.Contains(i)));

                filter = new FilterDefinitionBuilder<Branch>().And(filter, byCategoriesFilter);
            }

            if (!string.IsNullOrEmpty(partnerName))
            {
                partnerName = partnerName.ToLower().Trim();

                var byPartnerNameFilter = new FilterDefinitionBuilder<Branch>().Where(x =>
                    x.Name.Any(y => y.LocText.ToLower().Trim().Contains(partnerName)) ||
                    x.Description.Any(y => y.LocText.ToLower().Trim().Contains(partnerName)));

                filter = new FilterDefinitionBuilder<Branch>().And(filter, byPartnerNameFilter);
            }

            // TODO starting with mongoDb server 3.6+ versions we should use expr operator to get data in a one requst instead of two as below
            // var exprFilter = new BsonDocument("$expr", new BsonDocument("$eq", new BsonArray(new[] { "$_id", "$PartnerId" })));
            var partnerIds = await Collection.
            Aggregate().
            Match(filter).
            Group(x => x.PartnerId, g => new { g.Key }).ToListAsync();

            var ids = partnerIds.Select(p => p.Key).ToList();

            var filterByIds = new FilterDefinitionBuilder<Branch>().In(x => x.Id, ids);
            var filterSorting = new SortDefinitionBuilder<Branch>().Descending(b => b.Modified);

            var result = await Collection.
            Aggregate().
            Match(filterByIds).
            Sort(filterSorting).
            Project(p => new Branch
            {
                PartnerId = p.PartnerId,
                Name = p.Name,
                Description = p.Description,
                Address = p.Address,
                Location = p.Location,
                Phones = p.Phones,
                Timetable = p.Timetable,
                CategoryIds = p.CategoryIds,
                Url = p.Url,
                Discounts = p.Discounts,
                Modified = p.Modified,
                IsDeleted = p.IsDeleted
            }).ToListAsync();

            result.ForEach(e => e.Id = e.PartnerId);

            return result;
        }
    }
}
