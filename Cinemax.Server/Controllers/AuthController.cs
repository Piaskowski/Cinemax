using Cinemax.Application.Elements.Identity.Commands;
using Cinemax.Shared.Contracts.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cinemax.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("register")]
        public async Task<IActionResult> CreateAccount([FromBody] RegisterRequest request)
        {
            var command = new CreateAccountCommand
                (
                    request.FirstName, 
                    request.LastName, 
                    request.Email, 
                    request.Password
                );

            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> SignIn([FromBody] LoginRequest request)
        {
            var command = new SignInCommand
            (
                request.Email,
                request.Password
            );

            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
