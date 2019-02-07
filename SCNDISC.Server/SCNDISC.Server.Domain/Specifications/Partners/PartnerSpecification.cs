using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Exceptions.Partners;

namespace SCNDISC.Server.Domain.Specifications.Partners
{
    public class PartnerSpecification : BaseBranchSpecification
    {
        public override bool IsSatisfiedBy(Branch subject)
        {
            var result = base.IsSatisfiedBy(subject);
            if (subject.Id != subject.PartnerId)
            {
                throw new InvalidPartnerException("Partners Id and PartnerId must be equal");
            }
            return result;
        }
    }
}
