using Domain.Entity;
using Infrastructure.Context;
using Repository.Abstract;

namespace Repository.Implement
{
    public class UserUploadRepository : RepositoryBase<UserUpload>, IUserUploadRepository
    {
        public UserUploadRepository(ModuleCardDbContext context) : base(context)
        {
        }
    }
}
