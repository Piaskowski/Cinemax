using Cinemax.Shared.Enums;

namespace Cinemax.Shared.Contracts.TicketPrices
{
    public class SelectTicketPriceDto
    {
        public Guid Id { get; set; }
        public TicketType TicketType { get; set; }
        public SeatType SeatType { get; set; }
        public decimal Price { get; set; }
    }
}
