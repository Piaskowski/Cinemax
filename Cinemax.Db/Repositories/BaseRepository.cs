using Cinemax.Application.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Db.Repositories
{
    public class BaseRepository<TEntity, TKey>(DbContext dbContext) : IBaseRepository<TEntity, TKey>
        where TEntity : class
    {
        protected readonly DbContext _dbContext = dbContext;
        protected readonly DbSet<TEntity> DbSet = dbContext.Set<TEntity>();

        public async Task<int> CountAsync(CancellationToken ct = default)
        {
            return await DbSet.CountAsync(ct);
        }

        public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken ct = default)
        {
            DbSet.Add(entity);
            await _dbContext.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(TKey id, CancellationToken ct = default)
        {
            var entity = await GetByIdAsync(id, ct);

            if (entity is null)
                return false;

            DbSet.Remove(entity);
            await _dbContext.SaveChangesAsync(ct);

            return true;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default)
        {
            return await DbSet.ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default)
        {
            return await DbSet.FindAsync([id], ct);
        }

        public IQueryable<TEntity> Query()
        {
            return _dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> GetPaged(int pageNumber, int pageSize)
        {
            return Query()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken ct = default)
        {
            DbSet.Update(entity);
            await _dbContext.SaveChangesAsync(ct);
            return entity;
        }
    }
}
