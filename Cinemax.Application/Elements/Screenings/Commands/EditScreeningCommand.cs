using Cinemax.Shared.Contracts.Scrennings;
using MediatR;

namespace Cinemax.Application.Elements.Screenings.Commands
{
    public record EditScreeningCommand(EditScreeningRequest Request) : IRequest;
}
