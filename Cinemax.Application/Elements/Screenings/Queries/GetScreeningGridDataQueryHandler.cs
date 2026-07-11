using Cinemax.Application.Elements.Screenings.Repositories;
using Cinemax.Shared.Contracts.Movies;
using Cinemax.Shared.Contracts.CinemaHalls;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Scrennings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cinemax.Application.Elements.Screenings.Queries
{
    public class GetScreeningGridDataQueryHandler(IScreeningRepository repository,
        ILogger<GetScreeningGridDataQueryHandler> logger) : IRequestHandler<GetScreeningGridDataQuery, GridResponse<ScreeningDto>>
    {
        private readonly IScreeningRepository _repository = repository;
        private readonly ILogger _logger = logger;
        public async Task<GridResponse<ScreeningDto>> Handle(GetScreeningGridDataQuery query, CancellationToken cancellationToken)
        {
            var pageNumber = query.Request.PageNumber;
            var pageSize = query.Request.PageSize;

            var totalCount = await _repository.CountAsync(cancellationToken);
            var items = await _repository.GetPaged(pageNumber, pageSize)
                .Select(s => new ScreeningDto
                {
                    Id = s.Id,
                    Movie = new MovieSelectItemDto { Id = s.MovieId, Title = s.Movie.Title },
                    CinemaHall = new CinemaHallSelectItemDto { Id = s.CinemaHallId, Number = s.CinemaHall.Number },
                    StartTime = s.StartTime,
                    Status = s.Status
                }).ToListAsync(cancellationToken: cancellationToken);


            return new GridResponse<ScreeningDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }
    }
}
