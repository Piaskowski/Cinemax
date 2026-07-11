using Cinemax.Application.Abstractions.Interfaces;
using Cinemax.Domain.Entities;

namespace Cinemax.Application.Elements.Genres.Repositories
{
    public interface IGenreRepository : IBaseRepository<Genre, Guid>
    {
    }
}
