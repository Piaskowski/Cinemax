using Cinemax.Shared.Contracts.Movies;
using MediatR;

namespace Cinemax.Application.Elements.Movies.Queries
{
    public record GetMovieCarouselsQuery : IRequest<GetMovieCarouselsResponse>;
}
