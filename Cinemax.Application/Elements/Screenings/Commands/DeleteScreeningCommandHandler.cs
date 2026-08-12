using Cinemax.Application.Elements.Orders.Repositories;
using Cinemax.Application.Elements.Screenings.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Shared.Resources.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.Screenings.Commands
{
    public class DeleteScreeningCommandHandler(IScreeningRepository screenings,
        IOrderRepository orders) : IRequestHandler<DeleteScreeningCommand>
    {
        private readonly IScreeningRepository _screenings = screenings;
        private readonly IOrderRepository _orders = orders;
        public async Task Handle(DeleteScreeningCommand command, CancellationToken ct)
        {
            var hasOrders = await _orders.Query()
                .AnyAsync(o => o.ScreeningId == command.Id);

            if(hasOrders)
                throw new ValidationException([ValidationMessages.Screening_AssignedOrders]);

            await _screenings.DeleteAsync(command.Id, ct);
        }
    }
}
