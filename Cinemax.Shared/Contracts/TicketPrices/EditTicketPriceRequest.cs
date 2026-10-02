using Cinemax.Shared.Enums;

namespace Cinemax.Shared.Contracts.TicketPrices
{
    public class EditTicketPriceRequest
    {
        public Guid Id { get; set; }
        public ScreeningType ScreeningType { get; set; }
        public TicketType TicketType { get; set; }
        public SeatType SeatType { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }  
    }
}
