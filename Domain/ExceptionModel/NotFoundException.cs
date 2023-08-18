namespace Domain.ExceptionModel
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message)
        : base(message)
        {
        }
    }
}
