using Cinemax.Application.Elements.Movies.Repositories;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Contracts.Genres;
using Cinemax.Shared.Contracts.Movies;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cinemax.Application.Elements.Movies.Queries
{
    public class GetMovieGridDataQueryHandler(IMovieRepository repository,
        ILogger<GetMovieGridDataQueryHandler> logger) : IRequestHandler<GetMovieGridDataQuery, GridResponse<MovieDto>>
    {
        private readonly IMovieRepository _repository = repository;
        private readonly ILogger<GetMovieGridDataQueryHandler> _logger = logger;
        public async Task<GridResponse<MovieDto>> Handle(GetMovieGridDataQuery query, CancellationToken cancellationToken)
        {
            var pageNumber = query.Request.PageNumber;
            var pageSize = query.Request.PageSize;

            var totalCount = await _repository.CountAsync(cancellationToken);
            var items = await _repository.GetPaged(pageNumber, pageSize)
                .Select(m => new MovieDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Director = m.Director,
                    Description = m.Description,
                    DurationMinutes = m.DurationMinutes,
                    PosterUrl = m.PosterUrl,
                    TrailerUrl = m.TrailerUrl,
                    IsActive = m.IsActive,
                    Genres = m.Genres
                        .Select(g => new GenreDto
                        {
                            Id = g.Id,
                            Name = g.Name,
                            IsActive = g.IsActive
                        }).Where(g => g.IsActive)
                        .ToList()
                }).ToListAsync(cancellationToken: cancellationToken);


            return new GridResponse<MovieDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }
    }
}
