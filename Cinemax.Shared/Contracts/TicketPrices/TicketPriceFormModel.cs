using Cinemax.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cinemax.Shared.Contracts.TicketPrices
{
    public class TicketPriceFormModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Format jest wymagany.")]
        public ScreeningType ScreeningType { get; set; }

        [Required(ErrorMessage = "Typ biletu jest wymagany.")]
        public TicketType TicketType { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Cena musi być większa od 0")]
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
