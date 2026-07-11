using Cinemax.Shared.Enums;

namespace Cinemax.Shared.Contracts.Orders
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CreatedByUserEmail { get; set; }
        public Guid ScreeningId { get; set; }
        public DateTime ScreeningDate { get; set; }
        public int ReservationCount { get; set; }
        public OrderStatus Status { get; set; }
        public OrderSource Source { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime ExpiresAt { get; set; }
    }
}
