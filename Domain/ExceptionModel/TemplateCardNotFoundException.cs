namespace Domain.ExceptionModel
{
    public class TemplateCardNotFoundException : NotFoundException
    {
        public TemplateCardNotFoundException(string id) : base($"TemplateCardId '{id}' Not Found")
        {
        }
    }
}
