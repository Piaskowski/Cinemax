
namespace Cinemax.Application.Abstractions.Services
{
    public interface IPaymentService
    {
        Task<string> CreateCheckoutSessionAsync(Guid orderId,CancellationToken ct);
    }
}
