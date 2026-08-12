using Cinemax.Shared.Resources.Validation;
using System.ComponentModel.DataAnnotations;

namespace Cinemax.Shared.Contracts.Movies
{
    public class MovieFormModel
    {
        public Guid? Id { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.Required_Title))]
        public string Title { get; set; } = string.Empty;

        [Required(
            ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.Required_Director))]
        public string Director { get; set; } = string.Empty;

        [Required(
            ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.Required_Description))]
        public string Description { get; set; } = string.Empty;

        [Range(1, 1200, 
            ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.MinDuration))]
        public int DurationMinutes { get; set; }

        [MinLength(1, 
            ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.NotEmpty_GenresList))]
        public Guid[] GenresIds { get; set; } = [];
        public string? PosterUrl { get; set; }
        public string? TrailerUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
