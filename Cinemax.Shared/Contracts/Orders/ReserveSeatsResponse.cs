
namespace Cinemax.Shared.Contracts.Orders
{
    public class ReserveSeatsResponse
    {
        public Guid OrderId { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
