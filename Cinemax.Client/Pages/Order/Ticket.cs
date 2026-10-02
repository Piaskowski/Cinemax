using Cinemax.Shared.Contracts.Seats;
using Cinemax.Shared.Enums;

namespace Cinemax.Client.Pages.Order
{
    public class Ticket
    {
        public ScreeningSeatDto Seat { get; set; } = default!;
        public TicketType TicketType { get; set; }
        public decimal Price { get; set; }
    }
}
