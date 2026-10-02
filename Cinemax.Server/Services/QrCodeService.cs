using Cinemax.Application.Abstractions.Services;
using QRCoder;

namespace Cinemax.Server.Services
{
    public class QrCodeService : IQrCodeService
    {
        public byte[]? Generate(string? content)
        {
            if (string.IsNullOrEmpty(content))
                return null;

            using var generator = new QRCodeGenerator();

            using var data = generator.CreateQrCode(
                content,
                QRCodeGenerator.ECCLevel.Q);

            var qrCode = new PngByteQRCode(data);
            return qrCode.GetGraphic(20);
        }
    }
}
