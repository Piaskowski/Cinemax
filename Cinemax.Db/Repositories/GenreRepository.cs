using Cinemax.Application.Elements.Genres.Repositories;
using Cinemax.Db.Context;
using Cinemax.Domain.Entities;

namespace Cinemax.Db.Repositories
{
    public class GenreRepository(AppDbContext dbContext) : BaseRepository<Genre, Guid>(dbContext), IGenreRepository
    {
    }
}
