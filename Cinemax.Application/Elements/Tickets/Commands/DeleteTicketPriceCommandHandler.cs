using Cinemax.Application.Elements.Tickets.Repositories;
using MediatR;

namespace Cinemax.Application.Elements.Tickets.Commands
{
    public class DeleteTicketPriceCommandHandler(ITicketRepository tickets) : IRequestHandler<DeleteTicketPriceCommand>
    {
        private readonly ITicketRepository _tickets = tickets;
        public async Task Handle(DeleteTicketPriceCommand command, CancellationToken ct)
        {
            await _tickets.DeleteAsync(command.Id, ct);
        }
    }
}
