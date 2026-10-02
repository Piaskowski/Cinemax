
using Cinemax.Shared.Enums;

namespace Cinemax.Shared.Contracts.Orders
{
    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public string MovieTitle { get; set; } = default!;
        public string PosterUrl { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
    }
}
