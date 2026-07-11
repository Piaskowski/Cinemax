using Cinemax.Application.Abstractions.Interfaces;
using Cinemax.Domain.Entities;

namespace Cinemax.Application.Elements.Movies.Repositories
{
    public interface IMovieRepository : IBaseRepository<Movie, Guid>
    {
    }
}
