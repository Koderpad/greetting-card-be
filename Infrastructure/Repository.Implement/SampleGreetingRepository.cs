using Domain.Entity;
using Infrastructure.Context;
using Repository.Abstract;

namespace Repository.Implement
{
    public class SampleGreetingRepository : RepositoryBase<SampleGreeting>, ISampleGreetingRepository
    {
        public SampleGreetingRepository(ModuleCardDbContext context) : base(context)
        {
        }
    }
}
