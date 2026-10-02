
using Cinemax.Shared.Contracts.Identity;
using MediatR;

namespace Cinemax.Application.Elements.Identity.Commands
{
    public record ResetPasswordCommand(ResetPasswordRequest Request) : IRequest;
}
