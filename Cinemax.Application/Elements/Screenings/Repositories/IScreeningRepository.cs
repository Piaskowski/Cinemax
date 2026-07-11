using Cinemax.Application.Abstractions.Interfaces;
using Cinemax.Domain.Entities;

namespace Cinemax.Application.Elements.Screenings.Repositories
{
    public interface IScreeningRepository : IBaseRepository<Screening, Guid>
    {
    }
}
