using MediatR;

namespace Cinemax.Application.Elements.Screenings.Commands
{
    public record ImportScreeningsCommand(MemoryStream Stream) : IRequest<IEnumerable<string>>;
}
