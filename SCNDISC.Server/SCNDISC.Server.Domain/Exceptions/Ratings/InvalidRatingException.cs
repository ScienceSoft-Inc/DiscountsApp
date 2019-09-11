namespace SCNDISC.Server.Domain.Exceptions.Ratings
{
    class InvalidRatingException : DomainException
    {
        public InvalidRatingException(string message) : base(message) { }
    }
}
