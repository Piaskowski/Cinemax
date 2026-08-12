using Cinemax.Application.Elements.Tickets.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Domain.Entities.Orders;
using Cinemax.Shared.Resources.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.Tickets.Commands
{
    public class EditTicketPriceCommandHandler(ITicketRepository ticketPrices) : IRequestHandler<EditTicketPriceCommand>
    {
        private readonly ITicketRepository _ticketPrices = ticketPrices;
        public async Task Handle(EditTicketPriceCommand command, CancellationToken ct)
        {
            var request = command.Request;

            var exists = await _ticketPrices.Query()
                .AnyAsync(
                    t => t.ScreeningType == request.ScreeningType &&
                    t.TicketType == request.TicketType &&
                    t.Id != request.Id,
                    ct
                );

            if (exists)
                throw new ValidationException([ValidationMessages.TicketPrice_UnavailableScreeningAndTicketType]);

            var editedTicketPrice = await _ticketPrices.GetByIdAsync(request.Id, ct) ??
                throw new ValidationException([ValidationMessages.TicketPrice_NotExists]);

            editedTicketPrice.ScreeningType = request.ScreeningType;
            editedTicketPrice.TicketType = request.TicketType;
            editedTicketPrice.Price = request.Price;
            editedTicketPrice.IsActive = request.IsActive;

            await _ticketPrices.UpdateAsync(editedTicketPrice, ct);
        }
    }
}
