
using Cinemax.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cinemax.Shared.Contracts.CinemaHalls
{
    public class CinemaHallFormModel
    {
        public Guid? Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Numer musi być większy od 0.")]
        public int Number { get; set; }

        [Required(ErrorMessage = "Typ sali kinowej jest wymagany")]
        public CinemaHallType Type { get; set; }
        public bool IsActive { get; set; }
    }
}
