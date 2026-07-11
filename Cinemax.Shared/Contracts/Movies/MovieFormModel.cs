using System.ComponentModel.DataAnnotations;

namespace Cinemax.Shared.Contracts.Movies
{
    public class MovieFormModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Tytuł jest wymagany.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Reżyser jest wymagany.")]
        public string Director { get; set; } = string.Empty;

        [Required(ErrorMessage = "Opis jest wymagany.")]
        public string Description { get; set; } = string.Empty;

        [Range(1, 1200, ErrorMessage = "Czas trwania musi być większy od 0")]
        public int DurationMinutes { get; set; }

        [MinLength(1, ErrorMessage = "Wymagany przynajmniej jeden gatunek.")]
        public Guid[] GenresIds { get; set; } = [];
        public string? PosterUrl { get; set; }
        public string? TrailerUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
