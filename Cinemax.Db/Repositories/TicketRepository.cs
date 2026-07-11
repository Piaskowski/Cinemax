using Cinemax.Application.Elements.Tickets.Repositories;
using Cinemax.Db.Context;
using Cinemax.Domain.Entities.Orders;

namespace Cinemax.Db.Repositories
{
    public class TicketRepository(AppDbContext dbContext) : BaseRepository<TicketPrice, Guid>(dbContext), ITicketRepository
    {
    }
}
