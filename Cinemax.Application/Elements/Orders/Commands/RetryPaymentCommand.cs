using MediatR;

namespace Cinemax.Application.Elements.Orders.Commands
{
    public record RetryPaymentCommand(Guid OrderId) : IRequest;
}
