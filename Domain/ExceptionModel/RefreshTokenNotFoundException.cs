namespace Domain.ExceptionModel
{
    public class RefreshTokenNotFoundException : NotFoundException
    {
        public RefreshTokenNotFoundException(string id) : base($"RefreshTokenId '{id}' Not Found")
        {
        }
    }
}
