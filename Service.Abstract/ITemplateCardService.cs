using Domain.DTO;

namespace Service.Abstract
{
    public interface ITemplateCardService
    {
        List<TemplateCardDTO> GetAll();

        TemplateCardDTO GetById(string id);

        List<TemplateCardDTO> GetByName(string name);

        TemplateCardDTO Create(TemplateCardDTO dto);

        TemplateCardDTO Update(TemplateCardDTO dto);

        void Delete(string id);

        List<TemplateCardDTO> GetAllDeleted();

        TemplateCardDTO Restore(string id);
        void PermanentlyDelete(string id);
        PaginationData Get(int offset, int pagesize);
        List<TemplateCardDTO> GetByTag(string tagName);
    }
}
