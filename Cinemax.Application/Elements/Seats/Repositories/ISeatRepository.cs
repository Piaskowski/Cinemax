using Cinemax.Application.Abstractions.Interfaces;
using Cinemax.Domain.Entities;

namespace Cinemax.Application.Elements.Seats.Repositories
{
    public interface ISeatRepository : IBaseRepository<Seat, Guid>
    {
        Task<IEnumerable<Seat>> GetSeatsByCinemahall(Guid cinemahallId, CancellationToken ct);
    }
}
