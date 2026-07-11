using Cinemax.Application.Elements.Reservations.Repositories;
using Cinemax.Db.Context;
using Cinemax.Domain.Entities.Orders;

namespace Cinemax.Db.Repositories
{
    public class ReservationRepository(AppDbContext dbContext) : BaseRepository<Reservation, Guid>(dbContext), IReservationRepository
    {
    }
}
