using MediatR;

namespace Cinemax.Application.Elements.Orders.Commands
{
    public record MarkOrderAsPaidCommand(Guid OrderId) : IRequest;
}
