using Cinemax.Application.Elements.CinemaHalls.Repositories;
using Cinemax.Db.Context;
using Cinemax.Domain.Entities;

namespace Cinemax.Db.Repositories
{
    public class CinemaHallRepository(AppDbContext dbContext) : BaseRepository<CinemaHall, Guid>(dbContext), ICinemaHallRepository
    {
    }
}
