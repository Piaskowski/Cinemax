
using System.ComponentModel.DataAnnotations;

namespace Cinemax.Shared.Contracts.Movies
{
    public class CreateMovieRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Director { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public Guid[] GenresIds { get; set; } = [];
        public string? PosterUrl { get; set; }
        public string? TrailerUrl { get; set; }
    }
}
