using MediatR;

namespace Cinemax.Application.Elements.Identity.Commands
{
    public record GenerateChangePasswordNotificationCommand(string Email) : IRequest;
}
