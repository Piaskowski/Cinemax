using Cinemax.Application.Elements.CinemaHalls.Repositories;
using Cinemax.Shared.Contracts.CinemaHalls;
using Cinemax.Shared.Contracts.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Cinemax.Application.Elements.CinemaHalls.Queries
{
    public class GetCinemaHallGridDataQueryHandler(ICinemaHallRepository repository,
        ILogger<GetCinemaHallGridDataQueryHandler> logger) : IRequestHandler<GetCinemaHallGridDataQuery, GridResponse<CinemaHallDto>>
    {
        private readonly ICinemaHallRepository _repository = repository;
        private readonly ILogger<GetCinemaHallGridDataQueryHandler> _logger = logger;
        public async Task<GridResponse<CinemaHallDto>> Handle(GetCinemaHallGridDataQuery query, CancellationToken cancellationToken)
        {
            var pageNumber = query.Request.PageNumber;
            var pageSize = query.Request.PageSize;

            var totalCount = await _repository.CountAsync(cancellationToken);
            var items = await _repository.GetPaged(pageNumber, pageSize)
                .Select(c => new CinemaHallDto
                {
                    Id = c.Id,
                    Number = c.Number,
                    Type = c.Type,
                    SeatCount = c.Seats.Count,
                    IsActive = c.IsActive
                }).ToListAsync(cancellationToken: cancellationToken);


            return new GridResponse<CinemaHallDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }
    }
}
