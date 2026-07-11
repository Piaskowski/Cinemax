
using System.ComponentModel.DataAnnotations;

namespace Cinemax.Domain.Entities
{
    public class Movie
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(200)]
        public required string Title { get; set; }
        [Required]
        [MaxLength(200)]
        public required string Director { get; set; }
        [MaxLength(2000)]
        public string? Description { get; set; }
        public int DurationMinutes { get; set; }
        public ICollection<Genre> Genres { get; set; } = [];
        public string? PosterUrl { get; set; }
        public string? TrailerUrl { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
