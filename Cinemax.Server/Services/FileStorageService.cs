using Cinemax.Application.Abstractions.Interfaces;
using Cinemax.Application.Exceptions;
using System.Threading;

namespace Cinemax.Server.Services
{
    public class FileStorageService(IWebHostEnvironment env) : IFileStorageService
    {
        private readonly IWebHostEnvironment _env = env;
        public async Task<string> SaveMoviePosterAsync(IFormFile file, CancellationToken ct)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png"};

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
                throw new ValidationException(["Nieprawidłowe rozszerzenie pliku."]);

            var fileName = $"{Guid.NewGuid()}{extension}";

            var folderPath = Path.Combine(
                _env.WebRootPath,
                "uploads",
                "movies");

            Directory.CreateDirectory(folderPath);
            var filePath = Path.Combine(folderPath, fileName);

            await using var stream = File.Create(filePath);
            await file.CopyToAsync(stream, ct);

            return $"/uploads/movies/{fileName}";
        }
    }
}
