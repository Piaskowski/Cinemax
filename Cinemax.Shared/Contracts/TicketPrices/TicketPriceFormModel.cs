using Cinemax.Shared.Enums;
using Cinemax.Shared.Resources.Validation;
using System.ComponentModel.DataAnnotations;

namespace Cinemax.Shared.Contracts.TicketPrices
{
    public class TicketPriceFormModel
    {
        public Guid? Id { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.Required_ScreeningType))]
        public ScreeningType ScreeningType { get; set; }


        [Required(
            ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.Required_TicketType))]
        public TicketType TicketType { get; set; }

        [Range(0.01, double.MaxValue, 
            ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.MinPrice))]
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
