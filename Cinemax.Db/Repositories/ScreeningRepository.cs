using Cinemax.Application.Elements.Screenings.Repositories;
using Cinemax.Db.Context;
using Cinemax.Domain.Entities;

namespace Cinemax.Db.Repositories
{
    public class ScreeningRepository(AppDbContext dbContext) : BaseRepository<Screening, Guid>(dbContext), IScreeningRepository
    {
    }
}
