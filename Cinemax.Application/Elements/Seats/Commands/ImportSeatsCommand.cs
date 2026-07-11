using MediatR;

namespace Cinemax.Application.Elements.Seats.Commands
{
    public record ImportSeatsCommand(Guid CinemahallId, MemoryStream Stream) : IRequest<IEnumerable<string>>;
}
