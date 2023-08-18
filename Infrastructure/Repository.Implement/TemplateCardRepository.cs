using Domain.Entity;
using Infrastructure.Context;
using Repository.Abstract;

namespace Repository.Implement
{
    public class TemplateCardRepository : RepositoryBase<TemplateCard>, ITemplateCardRepository
    {
        public TemplateCardRepository(ModuleCardDbContext context) : base(context)
        {
        }
    }
}
