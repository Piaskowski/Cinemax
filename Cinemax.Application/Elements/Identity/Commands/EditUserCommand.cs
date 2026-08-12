using Cinemax.Shared.Contracts.Identity;
using MediatR;

namespace Cinemax.Application.Elements.Identity.Commands
{
    public record EditUserCommand(EditUserRequest Request) : IRequest;
}
