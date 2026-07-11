using Cinemax.Application.Abstractions.Interfaces;
using Cinemax.Application.Elements.Movies.Commands;
using Cinemax.Application.Elements.Movies.Queries;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Movies;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinemax.Server.Controllers.AdminPanel
{
    [Authorize(Roles = "Admin,Employee")]
    [ApiController]
    [Route("api/admin/[controller]")]
    public class MoviesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("list")]
        public async Task<IActionResult> GetMoviesSelectList()
        {
            var query = new GetMoviesSelectListQuery();

            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetMovies([FromBody] GridRequest gridRequest)
        {
            var query = new GetMovieGridDataQuery(gridRequest);

            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost("create-movie")]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieRequest request)
        {
            var command = new CreateMovieCommand(request);

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("edit-movie")]
        public async Task<IActionResult> EditMovie([FromBody] EditMovieRequest request)
        {
            var command = new EditMovieCommand(request);

            await _mediator.Send(command);
            return NoContent();
        }
    }
}
