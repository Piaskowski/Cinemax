using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Movies;
using MediatR;

namespace Cinemax.Application.Elements.Movies.Queries
{
    public record GetMovieGridDataQuery(GridRequest Request) : IRequest<GridResponse<MovieDto>>;
}
