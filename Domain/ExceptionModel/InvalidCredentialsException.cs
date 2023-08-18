namespace Domain.ExceptionModel
{
    public class InvalidCredentialsException : UnauthorizedException
    {
        public InvalidCredentialsException() : base("Invalid username or password")
        { }
    }
}
