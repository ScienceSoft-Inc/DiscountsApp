namespace SCNDISC.Server.Domain.Exceptions.Parameters
{
    public class InvalidParameterException : DomainException
    {
        public InvalidParameterException(string message) : base(message) { }
    }
}
