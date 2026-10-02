using Cinemax.Application.Elements.Identity.Commands;
using Cinemax.Application.Elements.Identity.Queries;
using Cinemax.Shared.Contracts.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinemax.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AccountController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAccountData() {
            var query = new GetMyAccountDataQuery();

            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] Guid userId, [FromQuery] string token)
        {
            var command = new ActivateAccountCommand(userId, token);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost("complete-registration")]
        public async Task<IActionResult> CompleteRegistration([FromQuery] Guid userId, [FromQuery] string emailToken,
            [FromQuery] string passwordToken, [FromBody] CompleteRegistrationRequest request)
        {
            var command = new CompleteRegistrationCommand(userId, 
                emailToken, passwordToken, request.Password);

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("generate-reset-password-notification")]
        public async Task<IActionResult> GenerateResetPasswordNotification([FromBody] GenerateResetPasswordNotificationRequest request)
        {
            var command = new GenerateChangePasswordNotificationCommand(request.Email);

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var command = new ResetPasswordCommand(request);

            await _mediator.Send(command);
            return NoContent();
        }
    }
}
