using Cinemax.Application.Elements.Seats.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinemax.Server.Controllers.AdminPanel
{
    [Authorize(Roles = "Admin,Employee")]
    [ApiController]
    [Route("api/admin/[controller]")]
    public class SeatsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

    }
}
