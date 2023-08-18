using Domain.Entity;
using Infrastructure.Context;
using Repository.Abstract;

namespace Repository.Implement
{
    public class UserCardRepository : RepositoryBase<UserCard>, IUserCardRepository
    {
        public UserCardRepository(ModuleCardDbContext context) : base(context)
        {
        }
    }
}
