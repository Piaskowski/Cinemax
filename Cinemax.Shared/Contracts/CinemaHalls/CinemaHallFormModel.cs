using Cinemax.Shared.Enums;
using Cinemax.Shared.Resources.Validation;
using System.ComponentModel.DataAnnotations;

namespace Cinemax.Shared.Contracts.CinemaHalls
{
    public class CinemaHallFormModel
    {
        public Guid? Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.MinValue_CinemaHall))]
        public int Number { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.Required_CinemaHallType))]
        public CinemaHallType Type { get; set; }
        public bool IsActive { get; set; }
    }
}
