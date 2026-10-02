
namespace Cinemax.Shared.Contracts.Movies
{
    public class GetMoviesResponse
    {
        public IEnumerable<MovieDisplayCardDto> Movies { get; set; } = [];
        public int TotalPages { get; set; }
    }
}
