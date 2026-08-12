using Cinemax.Application.Elements.Genres.Commands;
using Cinemax.Application.Elements.Tickets.Commands;
using Cinemax.Application.Elements.Tickets.Queries;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.TicketPrices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinemax.Server.Controllers.AdminPanel
{
    [Authorize(Roles = "Admin,Employee")]
    [ApiController]
    [Route("api/admin/[controller]")]
    public class TicketPricesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPost]
        public async Task<IActionResult> GetTicketPrices([FromBody] GridRequest gridRequest)
        {
            var query = new GetTicketPriceGridDataQuery(gridRequest);

            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost("create-ticket")]
        public async Task<IActionResult> CreateTicketPrice([FromBody] CreateTicketPriceRequest request)
        {
            var command = new CreateTicketPriceCommand(request);

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("edit-ticket")]
        public async Task<IActionResult> EditTicketPrice([FromBody] EditTicketPriceRequest request)
        {
            var command = new EditTicketPriceCommand(request);

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTicketPrice([FromRoute] Guid id)
        {
            var command = new DeleteTicketPriceCommand(id);

            await _mediator.Send(command);
            return NoContent();
        }
    }
}
