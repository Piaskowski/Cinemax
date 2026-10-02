using Cinemax.Application.Elements.Screenings.Repositories;
using Cinemax.Shared.Contracts.Movies;
using Cinemax.Shared.Contracts.Scrennings;
using Cinemax.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Cinemax.Application.Elements.Screenings.Queries
{
    public class GetSreeningsByDateQueryHandler(IScreeningRepository screenings) : IRequestHandler<GetSreeningsByDateQuery, GetScreeningsByDateResponse>
    {
        private readonly IScreeningRepository _screenings = screenings;
        public async Task<GetScreeningsByDateResponse> Handle(GetSreeningsByDateQuery query, CancellationToken ct)
        {
            var queryable = _screenings.Query()
                .Include(s => s.CinemaHall)
                .Include(s => s.Movie)
                .ThenInclude(m => m.Genres)
                .Where(s => DateOnly.FromDateTime(s.StartTime) == query.Date /*&& s.Status == ScreeningStatus.Scheduled*/);

            if (query.MovieId.HasValue && query.MovieId.Value != Guid.Empty)
            {
                queryable = queryable.Where(s => s.MovieId == query.MovieId.Value);
            }

            var screeningsByDate = await queryable.ToListAsync(ct);

            var groupedResult = screeningsByDate.GroupBy(s => s.Movie)
                .Select(g => new MovieShowtimesDto
                {
                    Id = g.Key.Id,
                    Title = g.Key.Title,
                    DurationMinutes = g.Key.DurationMinutes,
                    Genres = [.. g.Key.Genres.Select(x => x.Name)],
                    PosterUrl = g.Key.PosterUrl!,
                    Showtimes = [.. g.Select(s => new ShowtimeDto
                    {
                        Id = s.Id,
                        Type = s.CinemaHall.Type,
                        StartTime = s.StartTime,
                    })]
                }).ToList();

            return new GetScreeningsByDateResponse { 
                MovieShowtimes = groupedResult
            };
        }
    }
}
