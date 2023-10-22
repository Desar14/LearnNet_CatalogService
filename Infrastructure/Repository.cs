using LearnNet_CatalogService.Core.Interfaces;
using LearnNet_CatalogService.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        public async Task<int> Add(T entity)
        {
            EntityEntry<T> addedEntity = null;
            try
            {
                addedEntity = await _dbSet.AddAsync(entity);
                await Commit();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository {nameof(T)} add error");
                throw;
            }

            return addedEntity.Entity.Id;
        }

        public async Task<bool> Delete(int id)
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

            return await Commit();
        }

        public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>>? filter = null, int page = 0, int limit = 50)
        {
            try
            {
                IQueryable<T> query = _dbSet;

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                return await query.Skip(limit*page).Take(limit).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository {nameof(T)} get error");
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                return await _dbSet.ToListAsync();

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

        public async Task<bool> Update(T entity)
        {
            try
            {
                _dbSet.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository {nameof(T)} update error");
                throw;
            }

            return await Commit();
        }

        async Task<bool> Commit()
        {
            try
            {
                return (await _dbContext.SaveChangesAsync()) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository {nameof(T)} update error");
                throw;
            }
        }
    }
}
