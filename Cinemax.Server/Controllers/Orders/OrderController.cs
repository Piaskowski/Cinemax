using Cinemax.Application.Elements.Orders.Commands;
using Cinemax.Application.Elements.Orders.Queries;
using Cinemax.Application.Elements.Screenings.Queries;
using Cinemax.Application.Elements.Tickets.Queries;
using Cinemax.Shared.Contracts.Orders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinemax.Server.Controllers.Orders
{
    [ApiController]
    [Route("api/order")]
    public class OrderController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetScreeningSeats([FromRoute] Guid id)
        {
            var query = new GetSreeningSeatsQuery(id);

            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("get-ticket-prices")]
        public async Task<IActionResult> GetScreeningTickets([FromQuery] Guid screeningId)
        {
            var query = new GetScreeningTicketPricesQuery(screeningId);

            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost("get-order-details")]
        public async Task<IActionResult> GetOrderDetails([FromBody] NewOrderRequest request)
        {
            var result = await _mediator.Send(
                new GetOrderDetailsCommand(request));

            return Ok(result);
        }

        [HttpPost("purchase-tickets")]
        public async Task<IActionResult> CreateOrder([FromBody] NewOrderRequest request)
        {
            var result = await _mediator.Send(
                new PurchaseTicketsCommand(request));

            return Ok(result);
        }

        [HttpPost("{orderId:guid}/retry-payment")]
        public async Task<IActionResult> CreateOrder([FromRoute] Guid orderId)
        {
            await _mediator.Send(
                new RetryPaymentCommand(orderId));

            return Ok();
        }

        [HttpPost("reserve-seats")]
        public async Task<IActionResult> BookSeats([FromBody] NewOrderRequest request)
        {
            var result = await _mediator.Send(
                new BookSeatsCommand(request));

            return Ok(result);
        }

        [Authorize]
        [HttpGet("get-form-data")]
        public async Task<IActionResult> GetFormDataAsync()
        {
            var result = await _mediator.Send(
                    new GetUserOrderFormDataQuery()
                );

            return Ok(result);
        }

        [Authorize]
        [HttpGet("orders/{id:guid}")]
        public async Task<IActionResult> GetMovieDetails([FromRoute] Guid id)
        {
            var query = new GetUserOrderDetailsQuery(id);

            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("orders")]
        public async Task<IActionResult> GetUserOrders([FromQuery] int page)
        {
            var query = new GetUserOrderListQuery(page);

            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}
