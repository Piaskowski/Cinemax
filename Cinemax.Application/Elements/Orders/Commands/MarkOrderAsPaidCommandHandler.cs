using Cinemax.Application.Elements.Orders.Repositories;
using Cinemax.Application.Elements.Orders.Services;
using MediatR;

namespace Cinemax.Application.Elements.Orders.Commands
{
    public class MarkOrderAsPaidCommandHandler(IOrderService orderService) : IRequestHandler<MarkOrderAsPaidCommand>
    {
        private readonly IOrderService _orderService = orderService;
        public async Task Handle(MarkOrderAsPaidCommand command, CancellationToken ct)
        {
            await _orderService.MarkOrderAsConfirmed(command.OrderId, ct);
        }
    }
}
