namespace Domain.ExceptionModel
{
    public class UserCardNotFoundException : NotFoundException
    {
        public UserCardNotFoundException(string id) : base($"UserCardId '{id}' Not Found")
        {
        }
    }
}
