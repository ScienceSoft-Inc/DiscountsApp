namespace SCNDISC.Server.Domain.Exceptions.Categories
{
    public class InvalidCategoryException : DomainException
    {
        public InvalidCategoryException(string message) : base(message) { }
    }
}
