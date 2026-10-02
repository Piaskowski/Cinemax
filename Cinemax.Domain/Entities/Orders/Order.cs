using Cinemax.Domain.Entities.Identity;
using Cinemax.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cinemax.Domain.Entities.Orders
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        [EmailAddress]
        [MaxLength(320)]
        public string? CustomerEmail { get; set; }
        public Guid? CreatedByUserId { get; set; }
        public ApplicationUser? CreatedByUser { get; set; }
        public Guid ScreeningId { get; set; }
        public Screening Screening { get; set; } = default!;
        public ICollection<Reservation> Reservations { get; set; } = [];
        public OrderStatus Status { get; set; }
        public OrderSource Source { get; set; }
        [MaxLength(64)]
        public string? QrToken { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime ExpiresAt { get; set; }
    }
}
