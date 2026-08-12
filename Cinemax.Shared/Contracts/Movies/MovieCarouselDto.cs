
namespace Cinemax.Shared.Contracts.Movies
{
    public class MovieCarouselDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; } = string.Empty;
        public string? PosterUrl { get; set; }
    }
}
