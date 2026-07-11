
namespace Cinemax.Shared.Contracts.Genres
{
    public class GetGenresResponse
    {
        public IEnumerable<GenreDto> Items { get; set; } = [];
    }
}
