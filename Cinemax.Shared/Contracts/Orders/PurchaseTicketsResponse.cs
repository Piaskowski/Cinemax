
namespace Cinemax.Shared.Contracts.Orders
{
    public class PurchaseTicketsResponse
    {
        public Guid OrderId { get; set; }
        public string CheckoutUrl { get; set; } = null!;
    }
}
