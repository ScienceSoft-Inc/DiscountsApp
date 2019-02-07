namespace SCNDISC.Server.Domain.Specifications
{
    public interface ISpecification<in T>
    {
        bool IsSatisfiedBy(T subject);
    }
}
