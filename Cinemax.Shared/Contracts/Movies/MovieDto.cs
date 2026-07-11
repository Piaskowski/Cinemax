
using Cinemax.Shared.Contracts.Genres;

namespace Cinemax.Shared.Contracts.Movies
{
    public class MovieDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Director { get; set; }
        public string? Description { get; set; }
        public int DurationMinutes { get; set; }
        public ICollection<GenreDto> Genres { get; set; } = [];
        public string? PosterUrl { get; set; }
        public string? TrailerUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
