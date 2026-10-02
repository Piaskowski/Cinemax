using Cinemax.Shared.Enums;

namespace Cinemax.Shared.Contracts.TicketPrices
{
    public class CreateTicketPriceRequest
    {
        public ScreeningType ScreeningType { get; set; }
        public TicketType TicketType { get; set; }
        public SeatType SeatType { get; set; }
        public decimal Price { get; set; }
    }
}
