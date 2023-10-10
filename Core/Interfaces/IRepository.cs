using LearnNet_CatalogService.Domain.Common;
using LearnNet_CatalogService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
