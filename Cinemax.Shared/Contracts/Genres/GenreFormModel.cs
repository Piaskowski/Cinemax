using Cinemax.Shared.Resources.Validation;
using System.ComponentModel.DataAnnotations;

namespace Cinemax.Shared.Contracts.Genres
{
    public class GenreFormModel
    {
        public Guid? Id { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.Required_Name))]
        [MaxLength(
            50,
            ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.MaxLength))]
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
