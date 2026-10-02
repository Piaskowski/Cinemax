using Cinemax.Shared.Contracts.Orders;
using MediatR;

namespace Cinemax.Application.Elements.Orders.Commands
{
    public record PurchaseTicketsCommand(NewOrderRequest Request) : IRequest<PurchaseTicketsResponse>;
}
