
namespace Cinemax.Shared.Contracts.Movies
{
    public class GetMovieCarouselsResponse
    {
        public IEnumerable<MovieDisplayCardDto> MoviesOnScreen { get; set; } = [];
        public IEnumerable<MovieDisplayCardDto> MoviesComingSoon { get; set; } = [];
    }
}
