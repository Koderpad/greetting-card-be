using Domain.DTO;

namespace Service.Abstract
{
    public interface ICategoryService
    {
        List<CategoryDTO> GetAll();

        CategoryDTO GetById(string id);

        List<CategoryDTO> GetByName(string name);

        List<CategoryDTO> GetAllThemeCategory();

        CategoryDTO Create(CategoryDTO dto);

        CategoryDTO Update(CategoryDTO dto);

        void Delete(string id);

        List<CategoryDTO> GetAllDeleted();

        CategoryDTO Restore(string id);
        PaginationData GetTemplateCardByCategoryId(string categoryId, int offset, int pageSize);
        void PermanentlyDelete(string id);
        public PaginationData GetSampleGreetingsByCategoryId(string categoryId, int offset, int pageSize);
    }
}
