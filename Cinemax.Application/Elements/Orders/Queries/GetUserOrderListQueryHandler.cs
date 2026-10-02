using Cinemax.Application.Elements.Orders.Services;
using Cinemax.Shared.Contracts.Orders;
using Cinemax.Shared.Settings;
using MediatR;
using Microsoft.Extensions.Options;

namespace Cinemax.Application.Elements.Orders.Queries
{
    public class GetUserOrderListQueryHandler(IOrderService orderService, 
        IOptions<DisplaySettings> displaySettings) : IRequestHandler<GetUserOrderListQuery, GetUserOrderListQueryResponse>
    {
        private readonly IOrderService _orderService = orderService;
        private readonly int _ordersPerPage = displaySettings.Value.ItemsPerPage;
        public async Task<GetUserOrderListQueryResponse> Handle(GetUserOrderListQuery query, CancellationToken ct)
        {
            var userOrders = await _orderService.GetUserOrdersList(query.Page, _ordersPerPage, ct);
            var totalPages = await _orderService.GetUserOrdersListTotalPages(_ordersPerPage, ct);

            var items = userOrders.Select(o => new OrderItemDto
            {
                Id = o.Id,
                MovieTitle = o.Screening.Movie.Title,
                PosterUrl = o.Screening.Movie.PosterUrl!,
                CreatedAt = o.CreatedAt,
                Status = o.Status
            });

            return new GetUserOrderListQueryResponse
            {
                Items = [.. items],
                TotalPages = totalPages
            };
        }
    }
}
