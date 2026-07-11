using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.TicketPrices;
using MediatR;

namespace Cinemax.Application.Elements.Tickets.Queries
{
    public record GetTicketPriceGridDataQuery(GridRequest Request) : IRequest<GridResponse<TicketPriceDto>>;
}
