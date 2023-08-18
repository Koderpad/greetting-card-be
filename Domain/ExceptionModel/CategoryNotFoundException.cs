namespace Domain.ExceptionModel
{
    public class CategoryNotFoundException : NotFoundException
    {
        public CategoryNotFoundException(string id) : base($"CategoryId '{id}' Not Found")
        {
        }
    }
}
