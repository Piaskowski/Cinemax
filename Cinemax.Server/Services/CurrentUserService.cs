using Cinemax.Application.Abstractions.Interfaces;
using System.Security.Claims;

namespace Cinemax.Server.Services
{
    public class CurrentUserService(IHttpContextAccessor accessor) : ICurrentUserService
    {
        public string? Email => 
            accessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
    }
}
