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
			public IEnumerable<IEnumerable<Discount>> Discounts;
		}

	    public async Task<IEnumerable<Branch>> GetPartnersDiscountListAsync(DateTime? syncdate = null)
	    {
	        var groups = Collection.Aggregate().Group(x => x.PartnerId, g =>  new PartnerOnly
			{
	            PartnerId = g.Key,
		        Name = g.First(x => x.Id == x.PartnerId).Name,
				Address = g.First(x => x.Id == x.PartnerId).Address,
				Description = g.First(x => x.Id == x.PartnerId).Description,
				Icon = g.First(x => x.Id == x.PartnerId).Icon,
				Phones = g.First(x => x.Id == x.PartnerId).Phones,
				Timetable = g.First(x => x.Id == x.PartnerId).Timetable,
	            Categories = g.First(x => x.Id == x.PartnerId).CategoryIds,
				Url = g.First(x => x.Id == x.PartnerId).Url,
				IsDeleted = g.First(x => x.Id == x.PartnerId).IsDeleted,
				Modified = g.First(x => x.Id == x.PartnerId).Modified,
				WebAddresses = g.First(x => x.Id == x.PartnerId).WebAddresses,
	            Discounts = g.Select(x => x.Discounts)
	        });

		    IEnumerable<PartnerOnly> result;

	        if (syncdate != null)
	        {
	            syncdate = syncdate.Value.Kind == DateTimeKind.Utc ? syncdate : syncdate.Value.ToUniversalTime();
		        result = await groups.Match(i => i.Modified > syncdate).ToListAsync();
	        }
	        else
	        {
		        result = await groups.ToListAsync();
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
	            Discounts = g.Discounts.SelectMany(d => d),
		        WebAddresses = g.WebAddresses
			}).ToList();
	    }
	}
}
