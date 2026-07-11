
using System.ComponentModel.DataAnnotations;

namespace Cinemax.Shared.Contracts.Genres
{
    public class EditGenreRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
