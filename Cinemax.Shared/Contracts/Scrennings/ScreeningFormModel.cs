using Cinemax.Shared.Common.Annotations;
using Cinemax.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cinemax.Shared.Contracts.Scrennings
{
    public class ScreeningFormModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Film jest wymagany.")]
        public Guid MovieId { get; set; }

        [Required(ErrorMessage = "Sala jest wymagana.")]
        public Guid CinemaHallId { get; set; }

        [Required(ErrorMessage = "Data jest wymagana.")]
        [FutureDate(ErrorMessage = "Data musi być przyszła.")]
        public DateTime StartTime { get; set; }
        public ScreeningStatus Status { get; set; }
    }
}
