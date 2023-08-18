using Domain.Entity;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Repository.Abstract;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Repository.Implement
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class, IEntity
    {

        protected readonly ModuleCardDbContext context;

        protected RepositoryBase(ModuleCardDbContext context)
        {
            this.context = context;
        }

        public List<T> FindAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = context.Set<T>().Where(x => x.IsDeleted == false);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.AsNoTracking().ToList();
        }

        public List<T> FindByCondition(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = context.Set<T>().Where(expression);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.AsNoTracking().ToList();
        }

        public List<T> FindByConditionWithTracking(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = context.Set<T>().Where(expression);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.ToList();
        }

        public T FindById(string Id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = context.Set<T>().Where(x => x.Id == Id & x.IsDeleted == false);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.AsNoTracking().FirstOrDefault()!;
        }

        public void Create(T entity)
        {
            entity.IsDeleted = false;
            entity.CreateAt = DateTime.Now;
            context.Set<T>().Add(entity);
        }

        public void Update(T entity) => context.Set<T>().Update(entity);

        public void Delete(T entity) => context.Set<T>().Remove(entity);

        public void Save() => context.SaveChanges();

        public void Attach(Object entity) => context.Attach(entity);

        public IQueryable<T> QueryAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = context.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.AsNoTracking();
        }
    }
}
