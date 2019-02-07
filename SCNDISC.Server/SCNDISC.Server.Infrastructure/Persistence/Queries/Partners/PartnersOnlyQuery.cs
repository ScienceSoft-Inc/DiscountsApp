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
			var sortBuilder = new SortDefinitionBuilder<Branch>().Ascending(b => b.Id).Ascending(b => b.PartnerId);

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

			var groups = await Collection.
				Aggregate().
				Match(filter).
				Sort(sortBuilder).
				Group(x => x.PartnerId, g => new
				{
					PartnerId = g.Key,
					g.First(x => x.Id == x.PartnerId).Name,
					g.First(x => x.Id == x.PartnerId).Address,
					g.First(x => x.Id == x.PartnerId).Description,
					g.First(x => x.Id == x.PartnerId).Icon,
					g.First(x => x.Id == x.PartnerId).Image,
					g.First(x => x.Id == x.PartnerId).Phones,
					g.First(x => x.Id == x.PartnerId).Timetable,
					Categories = g.First(x => x.Id == x.PartnerId).CategoryIds,
					g.First(x => x.Id == x.PartnerId).Url,
					g.First(x => x.Id == x.PartnerId).Discounts,
					g.First(x => x.Id == x.PartnerId).Location,
					g.First(x => x.Id == x.PartnerId).Modified,
					g.First(x => x.Id == x.PartnerId).IsDeleted
				}).ToListAsync();

			return groups.Select(g => new Branch
			{
				Id = g.PartnerId,
				PartnerId = g.PartnerId,
				Name = g.Name,
				Description = g.Description,
				Address = g.Address,
				Location = g.Location,
				Icon = g.Icon,
				Image = g.Image,
				Phones = g.Phones,
				Timetable = g.Timetable,
				CategoryIds = g.Categories,
				Url = g.Url,
				Discounts = g.Discounts,
				Modified = g.Modified,
				IsDeleted = g.IsDeleted,
			}).ToList();

		}
	}
}
