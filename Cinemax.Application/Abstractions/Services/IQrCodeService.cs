
namespace Cinemax.Application.Abstractions.Services
{
    public interface IQrCodeService
    {
        byte[]? Generate(string? content);
    }
}
