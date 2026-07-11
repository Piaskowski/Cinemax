using Cinemax.Application.Elements.Seats.Repositories;
using Cinemax.Db.Context;
using Cinemax.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Db.Repositories
{
    public class SeatRepository(AppDbContext dbContext) : BaseRepository<Seat, Guid>(dbContext), ISeatRepository
    {
        public async Task<IEnumerable<Seat>> GetSeatsByCinemahall(Guid cinemahallId, CancellationToken ct)
        {
            return await Query()
                .Where(x => x.CinemaHallId.Equals(cinemahallId))
                .ToListAsync(ct);
        }
    }
}
