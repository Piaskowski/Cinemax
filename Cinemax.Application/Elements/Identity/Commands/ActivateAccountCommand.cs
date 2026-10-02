using MediatR;

namespace Cinemax.Application.Elements.Identity.Commands
{
    public record ActivateAccountCommand(Guid UserId, string Token) : IRequest<bool>;
}
