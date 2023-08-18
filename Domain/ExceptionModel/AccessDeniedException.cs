namespace Domain.ExceptionModel
{
    public class AccessDeniedException : ForbiddenException
    {
        public AccessDeniedException() : base("Access denied") { }
    }
}
