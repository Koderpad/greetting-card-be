using Domain.Entity;
using System.Linq.Expressions;

namespace Repository.Abstract
{
    public interface IRefreshTokenRepository
    {
        List<RefreshToken> FindAll(params Expression<Func<RefreshToken, object>>[] includes);

        List<RefreshToken> FindByCondition(Expression<Func<RefreshToken, bool>> expression, params Expression<Func<RefreshToken, object>>[] includes);

        void Create(RefreshToken entity);

        void Update(RefreshToken entity);

        void Delete(RefreshToken entity);

        void Save();

        RefreshToken FindById(string Id, params Expression<Func<RefreshToken, object>>[] includes);
    }
}
