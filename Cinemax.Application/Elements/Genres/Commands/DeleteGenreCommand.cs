using MediatR;

namespace Cinemax.Application.Elements.Genres.Commands
{
    public record DeleteGenreCommand(Guid Id) : IRequest;
}
