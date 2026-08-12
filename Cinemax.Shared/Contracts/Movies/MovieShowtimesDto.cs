using Cinemax.Shared.Contracts.Scrennings;

namespace Cinemax.Shared.Contracts.Movies
{
    public class MovieShowtimesDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public ICollection<string> Genres { get; set; } = [];
        public int DurationMinutes { get; set; }
        public string PosterUrl { get; set; } = default!;
        public ICollection<ShowtimeDto> Showtimes { get; set; } = [];
        
    }
}
