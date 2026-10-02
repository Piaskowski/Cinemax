using Cinemax.Application.Elements.Screenings.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cinemax.Server.Controllers
{
    [ApiController]
    [Route("api/showtimes")]
    public class MovieShowtimesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetMovieShowtimesByDate([FromQuery] DateOnly at, [FromQuery] Guid movie)
        {
            var query = new GetSreeningsByDateQuery(at, movie);

            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}
