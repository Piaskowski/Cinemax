using Cinemax.Application.Abstractions.Interfaces;
using Cinemax.Domain.Entities.Orders;

namespace Cinemax.Application.Elements.Orders.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order, Guid>
    {
    }
}
