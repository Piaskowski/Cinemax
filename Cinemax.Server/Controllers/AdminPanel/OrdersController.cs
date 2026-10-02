using Cinemax.Application.Elements.Orders.Commands;
using Cinemax.Application.Elements.Orders.Queries;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Orders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinemax.Server.Controllers.AdminPanel
{
    [Authorize(Roles = "Admin,Employee")]
    [ApiController]
    [Route("api/admin/[controller]")]
    public class OrdersController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPost]
        public async Task<IActionResult> GetOrders([FromBody] GridRequest gridRequest)
        {
            var query = new GetOrderGridDataQuery(gridRequest);

            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost("edit-order")]
        public async Task<IActionResult> EditOrder([FromBody] EditOrderRequest request)
        {
            var command = new EditOrderCommand(request);

            await _mediator.Send(command);
            return NoContent();
        }
    }
}
