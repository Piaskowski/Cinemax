using Cinemax.Application.Elements.Genres.Commands;
using Cinemax.Application.Elements.Genres.Queries;
using Cinemax.Application.Elements.Movies.Queries;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Genres;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinemax.Server.Controllers.AdminPanel
{
    [Authorize(Roles = "Admin,Employee")]
    [ApiController]
    [Route("api/admin/[controller]")]
    public class GenresController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAllGenres()
        {
            var query = new GetGenresQuery();

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetGenres([FromBody] GridRequest gridRequest)
        {
            var query = new GetGenreGridDataQuery(gridRequest);

            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost("create-genre")]
        public async Task<IActionResult> CreateGenre([FromBody] CreateGenreRequest request)
        {
            var command = new CreateGenreCommand(request);

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("edit-genre")]
        public async Task<IActionResult> EditGenre([FromBody] EditGenreRequest request)
        {
            var command = new EditGenreCommand(request);

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteGenre([FromRoute] Guid id)
        {
            var command = new DeleteGenreCommand(id);

            await _mediator.Send(command);
            return NoContent();
        }
    }
}
