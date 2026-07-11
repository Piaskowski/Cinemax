using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Orders;
using MediatR;

namespace Cinemax.Application.Elements.Orders.Queries
{
    public record GetOrderGridDataQuery(GridRequest Request) : IRequest<GridResponse<OrderDto>>;
}
