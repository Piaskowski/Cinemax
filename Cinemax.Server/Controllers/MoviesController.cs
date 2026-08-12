using Cinemax.Application.Elements.Movies.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cinemax.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("highlights")]
        public async Task<IActionResult> GetMovieSwipers()
        {
            var query = new GetMovieCarouselsQuery();

            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetMovieDetails([FromRoute] Guid id)
        {
            var query = new GetMovieDetailsQuery(id);

            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}
