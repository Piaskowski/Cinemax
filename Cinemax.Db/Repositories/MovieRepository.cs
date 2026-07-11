using Cinemax.Application.Elements.Movies.Repositories;
using Cinemax.Db.Context;
using Cinemax.Domain.Entities;

namespace Cinemax.Db.Repositories
{
    public class MovieRepository(AppDbContext dbContext) : BaseRepository<Movie, Guid>(dbContext), IMovieRepository
    {
    }
}
