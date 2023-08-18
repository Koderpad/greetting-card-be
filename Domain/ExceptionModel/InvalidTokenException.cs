namespace Domain.ExceptionModel
{
    public class InvalidTokenException : UnauthorizedException
    {
        public InvalidTokenException() : base("Invalid token") { }
    }
}
