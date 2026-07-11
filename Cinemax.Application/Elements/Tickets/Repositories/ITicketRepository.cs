using Cinemax.Application.Abstractions.Interfaces;
using Cinemax.Domain.Entities.Orders;

namespace Cinemax.Application.Elements.Tickets.Repositories
{
    public interface ITicketRepository : IBaseRepository<TicketPrice, Guid>
    {
    }
}
