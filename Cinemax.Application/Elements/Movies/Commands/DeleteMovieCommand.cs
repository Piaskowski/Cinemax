using MediatR;

namespace Cinemax.Application.Elements.Movies.Commands
{
    public record DeleteMovieCommand(Guid Id) : IRequest;
}
