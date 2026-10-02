using Cinemax.Application.Abstractions.Interfaces;
using Cinemax.Application.Elements.Orders.Repositories;
using Cinemax.Application.Elements.Reservations.Repositories;
using Cinemax.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.Orders.Commands
{
    public class UpdateOrdersStatusCommandHandler(IOrderRepository orders, 
        IReservationRepository reservations,
        IUnitOfWork unitOfWork) : IRequestHandler<UpdateOrdersStatusCommand>
    {
        private readonly IOrderRepository _orders = orders;
        private readonly IReservationRepository _reservations = reservations;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task Handle(UpdateOrdersStatusCommand command, CancellationToken ct)
        {
            var now = DateTime.UtcNow;

            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var expiredOrderIds = _orders.Query()
                    .Where(o =>
                        o.Status == OrderStatus.Pending &&
                        o.ExpiresAt <= now)
                    .Select(o => o.Id);

                await _reservations.Query()
                    .Where(r => expiredOrderIds.Contains(r.OrderId) && r.Status != ReservationStatus.Cancelled)
                    .ExecuteUpdateAsync(x =>
                        x.SetProperty(r => r.Status, ReservationStatus.Expired), ct);

                await _orders.Query()
                    .Where(o => expiredOrderIds.Contains(o.Id))
                    .ExecuteUpdateAsync(
                        x => x
                            .SetProperty(o => o.Status, OrderStatus.Expired), ct);

                await transaction.CommitAsync(ct);
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }
    }
}
