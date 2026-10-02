using Cinemax.Application.Abstractions.Interfaces;
using Cinemax.Application.Abstractions.Services;
using Cinemax.Application.Elements.Orders.Repositories;
using Cinemax.Application.Elements.Orders.Services;
using Cinemax.Shared.Contracts.Orders;
using MediatR;
using System.Security.Cryptography;

namespace Cinemax.Application.Elements.Orders.Commands
{
    public class PurchaseTicketsCommandHandler(IOrderRepository orders,
        IPaymentService paymentService,
        IOrderService orderService,
        IUnitOfWork unitOfWork) : IRequestHandler<PurchaseTicketsCommand, PurchaseTicketsResponse>
    {
        private readonly IOrderRepository _orders = orders;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IOrderService _orderService = orderService;

        private readonly IPaymentService _paymentService = paymentService;

        public async Task<PurchaseTicketsResponse> Handle(PurchaseTicketsCommand command, CancellationToken ct)
        {

            var request = command.Request;

            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                var order = await _orderService.PrepareOrderAsync(request, ct);
                order.ExpiresAt = DateTime.UtcNow.AddMinutes(15);

                order.QrToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
                await _orders.CreateAsync(order, ct);

                var checkoutUrl = await _paymentService.CreateCheckoutSessionAsync(
                    order.Id,
                    ct);

                await _unitOfWork.CommitTransactionAsync(ct);

                return new PurchaseTicketsResponse
                {
                    OrderId = order.Id,
                    CheckoutUrl = checkoutUrl
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
