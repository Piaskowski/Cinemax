using Cinemax.Application.Abstractions.Interfaces;
using Cinemax.Domain.Entities;

namespace Cinemax.Application.Elements.CinemaHalls.Repositories
{
    public interface ICinemaHallRepository : IBaseRepository<CinemaHall, Guid>
    {
    }
}
