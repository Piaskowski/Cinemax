using Cinemax.Application.Abstractions.Services;
using Cinemax.Application.Elements.Orders.Services;
using Cinemax.Domain.Entities.Orders;
using Cinemax.Shared.Contracts.Orders;
using Cinemax.Shared.Contracts.Reservations;
using MediatR;

namespace Cinemax.Application.Elements.Orders.Commands
{
    public class GetOrderDetailsCommandHandler(IOrderService orderService) : IRequestHandler<GetOrderDetailsCommand, OrderSummaryDto>
    {
        private readonly IOrderService _orderService = orderService;
        public async Task<OrderSummaryDto> Handle(GetOrderDetailsCommand command, CancellationToken ct)
        {
            var request = command.Request;
            var order = await _orderService.PrepareOrderAsync(request, ct);
            var tickets = order.Reservations
                .Select(r => new TicketDto { 
                    Price = r.FinalPrice, 
                    TicketType = r.TicketType,
                    SeatType = r.Seat.Type
                });

            return new OrderSummaryDto
            {
                Tickets = tickets,
                TotalPrice = tickets.Sum(t => t.Price)
            };
        }
    }
}
