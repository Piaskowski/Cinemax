
using Microsoft.AspNetCore.Http;

namespace Cinemax.Application.Abstractions.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveMoviePosterAsync(IFormFile file, CancellationToken ct);
    }
}
