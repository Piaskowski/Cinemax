using MediatR;

namespace Cinemax.Application.Elements.Identity.Commands
{
    public record CompleteRegistrationCommand(Guid UserId, string EmailToken, string PasswordToken, string Password) : IRequest;
}
