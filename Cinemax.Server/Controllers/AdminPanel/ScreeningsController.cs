using Cinemax.Application.Elements.Genres.Commands;
using Cinemax.Application.Elements.Screenings.Commands;
using Cinemax.Application.Elements.Screenings.Queries;
using Cinemax.Application.Elements.Seats.Commands;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Scrennings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinemax.Server.Controllers.AdminPanel
{
    [Authorize(Roles = "Admin,Employee")]
    [ApiController]
    [Route("api/admin/[controller]")]
    public class ScreeningsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> GetScreenings([FromBody] GridRequest gridRequest)
        {
            var query = new GetScreeningGridDataQuery(gridRequest);

            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost("create-screening")]
        public async Task<IActionResult> CreateScreening([FromBody] CreateScreeningRequest request)
        {
            var command = new CreateScreeningCommand(request);

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("edit-screening")]
        public async Task<IActionResult> EditScreening([FromBody] EditScreeningRequest request)
        {
            var command = new EditScreeningCommand(request);

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteScreening([FromRoute] Guid id)
        {
            var command = new DeleteScreeningCommand(id);

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("create-screenings")]
        public async Task<IActionResult> ImportScreenings([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Plik nie został poprawnie przesłany.");
            }

            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            var errors = await _mediator.Send(
                    new ImportScreeningsCommand(memoryStream));

            return errors.Any() ? BadRequest(errors) : NoContent();
        }
    }
}
