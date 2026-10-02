using Cinemax.Shared.Contracts.Orders;
using MediatR;

namespace Cinemax.Application.Elements.Orders.Queries
{
    public record GetUserOrderListQuery(int Page) : IRequest<GetUserOrderListQueryResponse>;

}
