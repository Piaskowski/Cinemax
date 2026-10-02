using Cinemax.Application.Elements.Screenings.Repositories;
using Cinemax.Application.Elements.Tickets.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Shared.Contracts.TicketPrices;
using Cinemax.Shared.Resources.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.Tickets.Queries
{
    public class GetScreeningTicketPricesQueryHandler(IScreeningRepository screenings,
        ITicketRepository tickets) : IRequestHandler<GetScreeningTicketPricesQuery, IEnumerable<SelectTicketPriceDto>>
    {
        private readonly IScreeningRepository _screenings = screenings;
        private readonly ITicketRepository _tickets = tickets;
        public async Task<IEnumerable<SelectTicketPriceDto>> Handle(GetScreeningTicketPricesQuery query, CancellationToken ct)
        {
            var screening = await _screenings.GetByIdAsync(query.ScreeningId, ct) ??
                throw new NotFoundException(ValidationMessages.NotFound_Screening);

            return await _tickets.Query()
                .Where(t => t.ScreeningType == screening.ScreeningType && t.IsActive)
                .Select(t => new SelectTicketPriceDto
                {
                    Id = t.Id,
                    TicketType = t.TicketType,
                    SeatType = t.SeatType,
                    Price = t.Price,
                }).ToListAsync(ct);
        }
    }
}
