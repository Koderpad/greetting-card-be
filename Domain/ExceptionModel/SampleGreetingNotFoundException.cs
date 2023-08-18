namespace Domain.ExceptionModel
{
    public class SampleGreetingNotFoundException : NotFoundException
    {
        public SampleGreetingNotFoundException(string id) : base($"SampleGreetingId '{id}' Not Found")
        {
        }
    }
}
