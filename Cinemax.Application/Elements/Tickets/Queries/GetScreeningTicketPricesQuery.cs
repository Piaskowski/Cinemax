using Cinemax.Shared.Contracts.TicketPrices;
using MediatR;

namespace Cinemax.Application.Elements.Tickets.Queries
{
    public record GetScreeningTicketPricesQuery(Guid ScreeningId) : IRequest<IEnumerable<SelectTicketPriceDto>>;
}
