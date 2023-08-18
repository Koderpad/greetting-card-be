namespace Domain.ExceptionModel
{
    public class UserUploadNotFoundException : NotFoundException
    {
        public UserUploadNotFoundException(string id) : base($"UserUploadId '{id}' Not Found")
        {
        }
    }
}
