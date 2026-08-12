using MediatR;

namespace Cinemax.Application.Elements.Identity.Commands
{
    public record DeleteUserCommand(Guid Id) : IRequest;
}
