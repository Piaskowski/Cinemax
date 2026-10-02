using Cinemax.Application.Exceptions;
using Cinemax.Domain.Entities.Identity;
using Cinemax.Shared.Contracts.Auth;
using Cinemax.Shared.Resources.Auth;
using Cinemax.Shared.Settings;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cinemax.Application.Elements.Identity.Commands
{
    public class SignInCommandHandler(UserManager<ApplicationUser> userManager, IOptions<AuthSettings> authSettings) : IRequestHandler<SignInCommand, LoginResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly AuthSettings _authSettings = authSettings.Value;
        public async Task<LoginResponse> Handle(SignInCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(command.Email) ?? 
                throw new ValidationException([AuthMessages.Error_WrongLoginOrPassword]);

            var isValidPassword = await _userManager.CheckPasswordAsync(user, command.Password);
            if (!isValidPassword)
                throw new ValidationException([AuthMessages.Error_WrongLoginOrPassword]);

            if (!user.EmailConfirmed)
                throw new ValidationException([AuthMessages.Error_EmailNotConfirmed]);

            if (!user.IsActive)
                throw new ValidationException([AuthMessages.Error_InactiveUser]);

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email!),
                new(ClaimTypes.GivenName, user.FirstName),
                new(ClaimTypes.Surname, user.LastName)
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var expiresAt = DateTime.UtcNow.AddDays(_authSettings.TokenExpirationDays);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _authSettings.Issuer,
                audience: _authSettings.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            return new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expiresAt
            };
        }
    }
}
