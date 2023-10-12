using LearnNet_CatalogService.Core.Interfaces;
using LearnNet_CatalogService.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace LearnNet_CatalogService.DataAccessSQL
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;
        protected readonly ILogger<Repository<T>> _logger;

        public Repository(ApplicationDbContext db, ILogger<Repository<T>> logger)
        {
            _dbContext = db;
            _dbSet = _dbContext.Set<T>();
            _logger = logger;
        }

        public async Task Add(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository {nameof(T)} add error");
                throw;
            }
        }

        public async Task<int> Commit()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                    throw new Exception();
                else
                    _dbSet.Remove(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository {nameof(T)} delete by id error");
                throw;
            }
        }

        public async Task<IQueryable<T>> GetAll()
        {
            try
            {
                IQueryable<T> query = _dbSet;

                return query;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository {nameof(T)} get error");
                throw;
            }
        }

        public async Task<T?> GetById(int id, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                var query = _dbSet.AsNoTracking();

                if (includes.Any())
                {
                    query = includes.Aggregate(query, (current, include) => current.Include(include));
                }

                return await query
                    .FirstOrDefaultAsync(entity => entity.Id.Equals(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository {nameof(T)} get by id error");
                throw;
            }
        }

        public Task Update(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository {nameof(T)} update error");
                throw;
            }
        }
    }
}
