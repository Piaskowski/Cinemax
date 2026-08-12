using MediatR;

namespace Cinemax.Application.Elements.Tickets.Commands
{
    public record DeleteTicketPriceCommand(Guid Id) : IRequest;
}
