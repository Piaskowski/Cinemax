using Cinemax.Application.Elements.Identity.Commands;
using Cinemax.Application.Elements.Identity.Queries;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinemax.Server.Controllers.AdminPanel
{
    [Authorize(Roles = "Admin,Employee")]
    [ApiController]
    [Route("api/admin/[controller]")]
    public class UsersController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> GetUsers([FromBody] GridRequest request)
        {
            var query = new GetUserGridDataQuery(request.PageNumber, request.PageSize);

            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var command = new CreateUserCommand(request);

            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("edit-user")]
        public async Task<IActionResult> CreateUser([FromBody] EditUserRequest request)
        {
            var command = new EditUserCommand(request);

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var command = new DeleteUserCommand(id);

            await _mediator.Send(command);
            return NoContent();
        }
    }
}
