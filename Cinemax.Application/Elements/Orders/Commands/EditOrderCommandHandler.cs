using Cinemax.Application.Abstractions.Interfaces;
using Cinemax.Application.Elements.Orders.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Shared.Enums;
using Cinemax.Shared.Resources.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.Orders.Commands
{
    public class EditOrderCommandHandler(IOrderRepository orders, IUnitOfWork unitOfWork) : IRequestHandler<EditOrderCommand>
    {
        private readonly IOrderRepository _orders = orders;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task Handle(EditOrderCommand command, CancellationToken ct)
        {
            var order = await _orders.Query()
                .Include(o => o.Reservations)
                .FirstOrDefaultAsync(o => o.Id == command.Request.Id, ct) ??
                    throw new NotFoundException(ValidationMessages.NotFound_Order);

            await _unitOfWork.BeginTransactionAsync(ct);
            if (order.Status != command.Request.Status)
            {
                if (order.Status != OrderStatus.Pending)
                {
                    await _unitOfWork.RollbackTransactionAsync(ct);
                    throw new ValidationException([string.Format(ValidationMessages.Error_CannotChangeStatus, order.Status)]);
                }
                    

                order.Status = command.Request.Status;
                foreach (var reservation in order.Reservations) {
                    if(reservation.Status == ReservationStatus.Pending)
                    {
                        reservation.Status = order.Status switch {
                            OrderStatus.Confirmed => ReservationStatus.Confirmed,
                            OrderStatus.Expired => ReservationStatus.Expired,
                            _ => ReservationStatus.Cancelled,
                        };
                    }
                }
                return;
            }

            var errors = new List<string>();
            foreach (var reservation in command.Request.Reservations) {

                var editedReservation = order.Reservations.FirstOrDefault(r => r.Id == reservation.Id);

                if (editedReservation == null) {
                    errors.Add(string.Format(ValidationMessages.Error_CannotChangeStatus, reservation.Id));
                    continue;
                }

                if (editedReservation.Status != reservation.Status)
                {
                    if(order.Status != OrderStatus.Pending)
                    {
                        await _unitOfWork.RollbackTransactionAsync(ct);
                        throw new ValidationException([string.Format(ValidationMessages.Error_CannotChangeOrderStatus, order.Status)]);
                    }
                        
                    if (editedReservation.Status != ReservationStatus.Pending)
                        errors.Add(string.Format(ValidationMessages.Error_CannotChangeReservationStatus, editedReservation.Status));

                    editedReservation.Status = reservation.Status;
                }
            }

            if (errors.Count != 0)
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
                throw new ValidationException(errors);
            }

            await _orders.UpdateAsync(order, ct);
            await _unitOfWork.CommitTransactionAsync(ct);
        }
    }
}
