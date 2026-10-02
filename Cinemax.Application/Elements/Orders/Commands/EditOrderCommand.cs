using Cinemax.Shared.Contracts.Orders;
using MediatR;

namespace Cinemax.Application.Elements.Orders.Commands
{
    public record EditOrderCommand(EditOrderRequest Request) : IRequest;
}
