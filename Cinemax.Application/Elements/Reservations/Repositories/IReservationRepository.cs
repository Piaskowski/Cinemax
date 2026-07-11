using Cinemax.Application.Abstractions.Interfaces;
using Cinemax.Domain.Entities.Orders;

namespace Cinemax.Application.Elements.Reservations.Repositories
{
    public interface IReservationRepository : IBaseRepository<Reservation, Guid>
    {
    }
}
