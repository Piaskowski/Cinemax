using Cinemax.Shared.Contracts.Reservations;

namespace Cinemax.Shared.Contracts.Orders
{
    public class OrderSummaryDto
    {
        public IEnumerable<TicketDto> Tickets { get; set; } = [];
        public decimal TotalPrice { get; set; }
    }
}
