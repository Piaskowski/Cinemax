
using System.ComponentModel.DataAnnotations;

namespace Cinemax.Domain.Entities
{
    public class Genre
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<Movie> Movies { get; set; } = [];
    }
}
