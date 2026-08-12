
namespace Cinemax.Shared.Contracts.Movies
{
    public class GetMovieCarouselsResponse
    {
        public IEnumerable<MovieCarouselDto> MoviesOnScreen { get; set; } = [];
        public IEnumerable<MovieCarouselDto> MoviesComingSoon { get; set; } = [];
    }
}
