using Domain.Entity;
using Infrastructure.Context;
using Repository.Abstract;

namespace Repository.Implement
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(ModuleCardDbContext context) : base(context)
        {
        }
    }
}
