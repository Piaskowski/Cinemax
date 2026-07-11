using Cinemax.Shared.Contracts.TicketPrices;
using MediatR;

namespace Cinemax.Application.Elements.Tickets.Commands
{
    public record CreateTicketPriceCommand(CreateTicketPriceRequest Request) : IRequest;
}
