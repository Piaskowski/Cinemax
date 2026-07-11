using Cinemax.Shared.Enums;

namespace Cinemax.Shared.Contracts.TicketPrices
{
    public class TicketPriceDto
    {
        public Guid Id { get; set; }
        public ScreeningType ScreeningType { get; set; }
        public TicketType TicketType { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
