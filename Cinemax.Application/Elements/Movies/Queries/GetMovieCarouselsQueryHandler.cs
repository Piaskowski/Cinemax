using Cinemax.Application.Elements.Movies.Repositories;
using Cinemax.Application.Elements.Screenings.Repositories;
using Cinemax.Shared.Contracts.Movies;
using Cinemax.Shared.Settings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Cinemax.Application.Elements.Movies.Queries
{
    public class GetMovieCarouselsQueryHandler(IMovieRepository movies,
        IScreeningRepository screenings,
        IOptions<HomeSettings> homeSettings) : IRequestHandler<GetMovieCarouselsQuery, GetMovieCarouselsResponse>
    {
        private readonly IMovieRepository _movies = movies;
        private readonly IScreeningRepository _screenings = screenings;
        private readonly int _moviesPerCarousel = homeSettings.Value.MoviesPerCarousel;
        public async Task<GetMovieCarouselsResponse> Handle(GetMovieCarouselsQuery query, CancellationToken ct)
        {
            var moviesOnScreen = await _screenings.Query()
                .Where(s => s.Status == Shared.Enums.ScreeningStatus.Scheduled && s.Movie.IsActive)
                .Take(_moviesPerCarousel)
                .Select(s => new MovieCarouselDto
                {
                    Id = s.Movie.Id,
                    Title = s.Movie.Title,
                    PosterUrl = s.Movie.PosterUrl
                })
                .ToListAsync();

            var movisComingSoon = await _movies.Query()
            .Where(m =>
                m.IsActive &&
                !_screenings.Query().Any(s => s.MovieId == m.Id))
            .Take(_moviesPerCarousel)
            .Select(m => new MovieCarouselDto
            {
                Id = m.Id,
                Title = m.Title,
                PosterUrl = m.PosterUrl
            })
            .ToListAsync(ct);

            return new GetMovieCarouselsResponse
            {
                MoviesOnScreen = moviesOnScreen,
                MoviesComingSoon = movisComingSoon,
            };
        }
    }
}
