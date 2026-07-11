using Cinemax.Shared.Contracts.Auth;
using MediatR;

namespace Cinemax.Application.Elements.Identity.Commands
{
    public record SignInCommand(string Email, string Password) : IRequest<LoginResponse>;
}
