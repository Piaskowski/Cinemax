using Cinemax.Shared.Contracts.Reservations;
using Cinemax.Shared.Enums;

namespace Cinemax.Shared.Contracts.Orders
{
    public class OrderEditFormModel
    {
        public Guid Id { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CreatedByUserEmail { get; set; }
        public DateTime ScreeningDate { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public bool CanEditStatus { get; set; }
        public List<OrderReservationFormModel> Reservations { get; set; } = [];
    }
}
