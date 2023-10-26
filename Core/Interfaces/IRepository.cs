using LearnNet_CatalogService.Data.Common;
using System.Linq.Expressions;

namespace LearnNet_CatalogService.Core.Interfaces
{
    public interface IRepository<T, TKey> where T : BaseEntity<TKey> 
                                          where TKey : struct
    {
        public Task<IList<T>> GetAll();
        public Task<IList<T>> Get(Expression<Func<T, bool>>? filter = null, int page = 0, int limit = 50);
        public Task<T?> GetById(int id, params Expression<Func<T, object>>[] includes);
        public Task<T> Add(T entity);
        public Task<bool> Update(T entity);
        public Task<bool> Delete(int id);
    }
}
