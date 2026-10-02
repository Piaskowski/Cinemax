using Cinemax.Application.Elements.Orders.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace Cinemax.Server.Controllers.Orders
{
    [ApiController]
    [Route("api/stripe/webhook")]
    public class StripeWebhookController(
        IConfiguration configuration,
        IMediator mediator) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Handle()
        {
            var webhookSecret = _configuration["Stripe:WebhookSecret"]
            ?? throw new InvalidOperationException(
                "Stripe WebhookSecret is not configured.");

            var json = await new StreamReader(Request.Body)
                .ReadToEndAsync();

            Event stripeEvent;

            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    webhookSecret);
            }
            catch (StripeException)
            {
                return BadRequest();
            }

            if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Session;

                if (session?.PaymentStatus == "paid" &&
                    Guid.TryParse(session.ClientReferenceId, out var orderId))
                {
                    await _mediator.Send(
                        new MarkOrderAsPaidCommand(orderId));
                }
            }

            return NoContent();
        }
    }
}
