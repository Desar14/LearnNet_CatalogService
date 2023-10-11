using LearnNet_CatalogService.Domain.Common;
using System.Linq.Expressions;

namespace LearnNet_CatalogService.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        public Task<IQueryable<T>> GetAll();
        public Task<T?> GetById(int id, params Expression<Func<T, object>>[] includes);
        public Task Add(T entity);
        public Task Update(T entity);
        public Task Delete(int id);
    }
}
