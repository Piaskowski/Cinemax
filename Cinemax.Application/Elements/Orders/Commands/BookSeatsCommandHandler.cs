using Cinemax.Application.Abstractions.Interfaces;
using Cinemax.Application.Elements.Orders.Repositories;
using Cinemax.Application.Elements.Orders.Services;
using Cinemax.Shared.Contracts.Orders;
using MediatR;
using System.Security.Cryptography;

namespace Cinemax.Application.Elements.Orders.Commands
{
    public class BookSeatsCommandHandler(IOrderRepository orders,
        IOrderService orderService,
        IUnitOfWork unitOfWork) : IRequestHandler<BookSeatsCommand, ReserveSeatsResponse>
    {
        private readonly IOrderRepository _orders = orders;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IOrderService _orderService = orderService;

        public async Task<ReserveSeatsResponse> Handle(BookSeatsCommand command, CancellationToken ct)
        {
            var request = command.Request;

            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                var order = await _orderService.PrepareOrderAsync(request, ct);
                order.ExpiresAt = DateTime.UtcNow.AddMinutes(15);
                order.QrToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));

                await _orders.CreateAsync(order, ct);
                await _unitOfWork.CommitTransactionAsync(ct);

                return new ReserveSeatsResponse
                {
                    OrderId = order.Id,
                    ExpiresAt = order.ExpiresAt
                };
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
                throw;
            }
        }
    }
}
