using System.ComponentModel.DataAnnotations;

namespace Cinemax.Shared.Contracts.Genres
{
    public class GenreFormModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana.")]
        [MaxLength(50, ErrorMessage = "Maksymalna długość znaków: 50.")]
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
