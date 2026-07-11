using Cinemax.Shared.Contracts.CinemaHalls;
using MediatR;

namespace Cinemax.Application.Elements.CinemaHalls.Queries
{
    public record GetCinemaHallSelectListQuery : IRequest<IEnumerable<CinemaHallSelectItemDto>>;
}
