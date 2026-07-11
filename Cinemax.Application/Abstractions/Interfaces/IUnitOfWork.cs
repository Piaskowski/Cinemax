using Microsoft.EntityFrameworkCore.Storage;

namespace Cinemax.Application.Abstractions.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken ct = default);

        Task<IDbContextTransaction> BeginTransactionAsync(
            CancellationToken ct = default);

        Task CommitTransactionAsync(
            CancellationToken ct = default);

        Task RollbackTransactionAsync(
            CancellationToken ct = default);
    }
}
