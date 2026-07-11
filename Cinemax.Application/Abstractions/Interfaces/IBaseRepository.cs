namespace Cinemax.Application.Abstractions.Interfaces
{
    public interface IBaseRepository<TEntity, TKey>
        where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default);
        Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default);
        Task<TEntity> CreateAsync(TEntity entity, CancellationToken ct = default);
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken ct = default);
        Task<int> CountAsync(CancellationToken ct = default);
        Task<bool> DeleteAsync(TKey id, CancellationToken ct = default);
        IQueryable<TEntity> Query();
        IQueryable<TEntity> GetPaged(int pageNumber, int pageSize);

    }
}
