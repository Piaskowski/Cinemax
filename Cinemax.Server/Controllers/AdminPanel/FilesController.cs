using Cinemax.Application.Abstractions.Interfaces;
using Cinemax.Shared.Contracts.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinemax.Server.Controllers.AdminPanel
{
    [Authorize(Roles = "Admin,Employee")]
    [ApiController]
    [Route("api/admin/[controller]")]
    public class FilesController(IFileStorageService storageService) : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService = storageService;

        [HttpPost("add-movie-poster")]
        public async Task<IActionResult> UploadMoviePoster(IFormFile file, CancellationToken ct)
        {
            var url = await _fileStorageService.SaveMoviePosterAsync(
                file,
                ct);

            return Ok(new FileUploadResponse
            {
                Url = url
            });
        }
    }
}
