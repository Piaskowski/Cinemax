using Cinemax.Shared.Contracts.Auth;
using MediatR;

namespace Cinemax.Application.Elements.Identity.Commands
{
    public record CreateAccountCommand(string FirstName, string LastName, string Email, string Password) : IRequest<RegisterResponse>;

}
