using Cinemax.Application.Elements.CinemaHalls.Commands;
using Cinemax.Application.Elements.CinemaHalls.Queries;
using Cinemax.Application.Elements.Seats.Commands;
using Cinemax.Shared.Contracts.CinemaHalls;
using Cinemax.Shared.Contracts.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinemax.Server.Controllers.AdminPanel
{
    [Authorize(Roles = "Admin,Employee")]
    [ApiController]
    [Route("api/admin/[controller]")]
    public class CinemaHallsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("list")]
        public async Task<IActionResult> GetCinemaHallSelectList()
        {
            var query = new GetCinemaHallSelectListQuery();

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetCinemaHalls([FromBody] GridRequest gridRequest)
        {
            var query = new GetCinemaHallGridDataQuery(gridRequest);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("create-cinemahall")]
        public async Task<IActionResult> CreateCinemaHall([FromBody] CreateCinemaHallRequest request)
        {
            var command = new CreateCinemaHallCommand(request);

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("edit-cinemahall")]
        public async Task<IActionResult> EditCinemaHall([FromBody] EditCinemaHallRequest request)
        {
            var command = new EditCinemaHallCommand(request);

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("{cinemaHallId:guid}/create-seats")]
        public async Task<IActionResult> CreateSeats([FromRoute] Guid cinemaHallId, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Plik nie został poprawnie przesłany.");
            }

            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            var errors = await _mediator.Send(
                    new ImportSeatsCommand(cinemaHallId, memoryStream));

            return errors.Any() ? BadRequest(errors) : NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCinemaHall([FromRoute] Guid id)
        {
            var command = new DeleteCinemaHallCommand(id);

            await _mediator.Send(command);
            return NoContent();
        }
    }
}
