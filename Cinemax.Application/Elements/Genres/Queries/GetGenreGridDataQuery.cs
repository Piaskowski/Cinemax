using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Genres;
using MediatR;

namespace Cinemax.Application.Elements.Genres.Queries
{
    public record GetGenreGridDataQuery(GridRequest Request) : IRequest<GridResponse<GenreDto>>;
}
