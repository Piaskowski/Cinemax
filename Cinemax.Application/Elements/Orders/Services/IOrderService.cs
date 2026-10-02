using Cinemax.Application.Abstractions.Interfaces;
using Cinemax.Domain.Entities.Orders;
using Cinemax.Shared.Contracts.Orders;

namespace Cinemax.Application.Elements.Orders.Services
{
    public interface IOrderService
    {
        Task<Order> PrepareOrderAsync(NewOrderRequest request, CancellationToken ct);
        Task MarkOrderAsConfirmed(Guid orderId, CancellationToken ct);
        Task<Order?> GetUserOrder(Guid id, CancellationToken ct);
        Task<List<Order>> GetUserOrdersList(int page, int itemsPerPage, CancellationToken ct);
        Task<int> GetUserOrdersListTotalPages(int ordersPerPage, CancellationToken ct);
    }
}
