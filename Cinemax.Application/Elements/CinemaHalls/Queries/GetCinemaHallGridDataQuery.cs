using Cinemax.Shared.Contracts.CinemaHalls;
using Cinemax.Shared.Contracts.Common;
using MediatR;

namespace Cinemax.Application.Elements.CinemaHalls.Queries
{
    public record GetCinemaHallGridDataQuery(GridRequest Request) : IRequest<GridResponse<CinemaHallDto>>;
}
