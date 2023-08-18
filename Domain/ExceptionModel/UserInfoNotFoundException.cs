namespace Domain.ExceptionModel
{
    public class UserInfoNotFoundException : NotFoundException
    {
        public UserInfoNotFoundException(string id) : base($"UserInfoId '{id}' Not Found")
        {
        }
    }
}
