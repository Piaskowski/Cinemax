using Cinemax.Application.Elements.CinemaHalls.Repositories;
using Cinemax.Shared.Contracts.CinemaHalls;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.CinemaHalls.Queries
{
    public class GetCinemaHallSelectListQueryHandler(ICinemaHallRepository halls) : IRequestHandler<GetCinemaHallSelectListQuery, IEnumerable<CinemaHallSelectItemDto>>
    {
        private readonly ICinemaHallRepository _halls = halls;

        public async Task<IEnumerable<CinemaHallSelectItemDto>> Handle(GetCinemaHallSelectListQuery query, CancellationToken ct)
        {
            return await _halls.Query()
                .Select(c => new CinemaHallSelectItemDto
                {
                    Id = c.Id,
                    Number = c.Number
                }).ToListAsync(ct);
        }
    }
}
