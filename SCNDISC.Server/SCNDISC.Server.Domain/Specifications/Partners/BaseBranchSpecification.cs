using System.Linq;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Exceptions.Partners;

namespace SCNDISC.Server.Domain.Specifications.Partners
{
    public abstract class BaseBranchSpecification : ISpecification<Branch>
    {
        public virtual bool IsSatisfiedBy(Branch subject)
        {
            if (subject == null)
            {
                throw new InvalidBranchException("branch must not be null");
            }

	        if (subject.IsDeleted)
	        {
		        throw new InvalidBranchException("Cannot modify deleted branch");
			}

            if (subject.Location == null || subject.Location.Coordinates == null
                || (subject.Location.Coordinates.Latitude == default(double) || subject.Location.Coordinates.Longitude == default(double)))
            {
                throw new InvalidBranchException("branch must have valid coordinates");
            }
            if (subject.Discounts == null || !subject.Discounts.Any())
            {
                throw new InvalidBranchException("branch must have discounts");
            }
            return true;
        }
    }
}
