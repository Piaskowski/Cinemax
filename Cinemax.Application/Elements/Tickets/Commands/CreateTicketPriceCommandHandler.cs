using Cinemax.Application.Elements.Tickets.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Domain.Entities.Orders;
using Cinemax.Shared.Resources.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.Tickets.Commands
{
    public class CreateTicketPriceCommandHandler(ITicketRepository repository) : IRequestHandler<CreateTicketPriceCommand>
    {
        private readonly ITicketRepository _repository = repository;

        public async Task Handle(CreateTicketPriceCommand command, CancellationToken ct)
        {
            var request = command.Request;

            var exists = await _repository.Query()
                .AnyAsync(
                    t => t.ScreeningType == request.ScreeningType &&
                    t.TicketType == request.TicketType &&
                    t.SeatType == request.SeatType,
                    ct
                );

            if (exists)
                throw new ValidationException([ValidationMessages.TicketPrice_UnavailableScreeningAndTicketType]);

            var ticketPrice = new TicketPrice
            {
                ScreeningType = request.ScreeningType,
                TicketType = request.TicketType,
                SeatType = request.SeatType,
                Price = request.Price,
            };

            await _repository.CreateAsync(ticketPrice, ct);
        }
    }
}
