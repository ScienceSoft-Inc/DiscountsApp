using System;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Exceptions.Partners;

namespace SCNDISC.Server.Domain.Specifications.Partners
{
    public class BranchSpecification : BaseBranchSpecification
    {
        public override bool IsSatisfiedBy(Branch subject)
        {
            var result = base.IsSatisfiedBy(subject);
            if (String.IsNullOrEmpty(subject.PartnerId))
            {
                throw new InvalidBranchException("Branch must have PartnerId");
            }
            return result;
        }
    }
}
