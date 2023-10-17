using LearnNet_CatalogService.Data.Common;
using System.Linq.Expressions;

namespace LearnNet_CatalogService.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        public Task<IEnumerable<T>> GetAll();
        public Task<T?> GetById(int id, params Expression<Func<T, object>>[] includes);
        public Task<bool> Add(T entity);
        public Task<bool> Update(T entity);
        public Task<bool> Delete(int id);
    }
}
