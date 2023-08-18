using Domain.Entity;
using Infrastructure.Context;
using Repository.Abstract;
using Repository.Implement;

namespace Infrastructure.Repository.Implement
{
    public class TagRepository : RepositoryBase<Tag>, ITagRepository
    {
        public TagRepository(ModuleCardDbContext context) : base(context)
        {
        }
    }
}
