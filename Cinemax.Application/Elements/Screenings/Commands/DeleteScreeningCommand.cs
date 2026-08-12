using MediatR;

namespace Cinemax.Application.Elements.Screenings.Commands
{
    public record DeleteScreeningCommand(Guid Id) : IRequest;
}
