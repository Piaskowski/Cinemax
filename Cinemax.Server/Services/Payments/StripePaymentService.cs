using Cinemax.Application.Abstractions.Services;
using Cinemax.Application.Elements.Orders.Repositories;
using Cinemax.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

namespace Cinemax.Server.Services.Payments
{
    public class StripePaymentService(IOrderRepository orders,
        StripeClient client,
        IConfiguration configuration) : IPaymentService
    {
        private readonly StripeClient _client = client;
        private readonly IOrderRepository _orders = orders;
        private readonly string _domain = configuration["App:Url"]
            ?? throw new InvalidOperationException(
                "App URL is not configured.");

        public async Task<string> CreateCheckoutSessionAsync(Guid orderId, CancellationToken ct)
        {
            var order = await _orders.Query()
                .Include(o => o.Reservations)
                .FirstOrDefaultAsync(o => o.Id == orderId, ct) ?? throw new ValidationException(["Zamówienie nie zostało utworzone"]);

            var options = new SessionCreateOptions
            {
                Mode = "payment",

                ClientReferenceId = orderId.ToString(),

                SuccessUrl = $"{_domain}/payment/result?orderId={order.Id}&result=success",

                CancelUrl = $"{_domain}/payment/result?orderId={order.Id}&result=cancel",

                LineItems = [.. order.Reservations.Select(r => new SessionLineItemOptions
                {
                    Quantity = 1,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "PLN",
                        UnitAmount = (long)(r.FinalPrice * 100),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"Bilet {r.TicketType}, miejsce {r.Seat.Type}",
                        }
                    }
                })]
            };

            var session =
                await _client.V1.Checkout.Sessions.CreateAsync(
                    options,
                    cancellationToken: ct);

            return session.Url;
        }
    }
}
