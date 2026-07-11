using Cinemax.Application.Elements.Orders.Repositories;
using Cinemax.Db.Context;
using Cinemax.Domain.Entities.Orders;


namespace Cinemax.Db.Repositories
{
    public class OrderRepository(AppDbContext dbContext) : BaseRepository<Order, Guid>(dbContext), IOrderRepository
    {
    }
}
