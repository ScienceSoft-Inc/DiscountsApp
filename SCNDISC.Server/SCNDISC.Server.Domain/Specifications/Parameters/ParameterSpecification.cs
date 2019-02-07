using SCNDISC.Server.Domain.Aggregates.Parameters;
using SCNDISC.Server.Domain.Exceptions.Parameters;

namespace SCNDISC.Server.Domain.Specifications.Parameters
{
    public class ParameterSpecification : ISpecification<Parameter>
    {
        public bool IsSatisfiedBy(Parameter subject)
        {
            if (subject == null)
            {
                throw new InvalidParameterException("parameter must not be null");
            }

            if (string.IsNullOrWhiteSpace(subject.Key))
            {
                throw new InvalidParameterException("parameters' key must not be null or empty");
            }

            return true;
        }
    }
}
