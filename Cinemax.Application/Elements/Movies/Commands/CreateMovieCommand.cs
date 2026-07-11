using Cinemax.Shared.Contracts.Movies;
using MediatR;

namespace Cinemax.Application.Elements.Movies.Commands
{
    public record CreateMovieCommand(CreateMovieRequest Request) : IRequest;
}
