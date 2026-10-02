using Cinemax.Application.Abstractions.Interfaces;
using System.Security.Claims;

namespace Cinemax.Server.Services
{
    public class CurrentUserService(IHttpContextAccessor accessor) : ICurrentUserService
    {
        public Guid? Id
        {
            get
            {
                var value = accessor.HttpContext?
                    .User
                    .FindFirstValue(ClaimTypes.NameIdentifier);

                return Guid.TryParse(value, out var id)
                    ? id
                    : null;
            }
        }

        public string? Email => 
            accessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
    }
}
