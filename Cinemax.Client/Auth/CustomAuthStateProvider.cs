using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Cinemax.Client.Auth
{
    public class CustomAuthStateProvider(ITokenStorage tokenStorage) : AuthenticationStateProvider
    {
        private readonly ITokenStorage _tokenStorage = tokenStorage;
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _tokenStorage.GetTokenAsync();

            if (string.IsNullOrEmpty(token)) 
            {   
                return GetAnonymousState();
            }
            else if (IsTokenExpired(token)){
                await _tokenStorage.RemoveTokenAsync();
                return GetAnonymousState();
            }

            var user = SetClaimsPrincipal(token);

            return new AuthenticationState(user);
        }

        public async Task MarkUserAsAuthenticated(string token)
        {
            await _tokenStorage.SetTokenAsync(token);
            var user = SetClaimsPrincipal(token);

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(user)));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _tokenStorage.RemoveTokenAsync();

            NotifyAuthenticationStateChanged(
                Task.FromResult(GetAnonymousState()));
        }

        private static AuthenticationState GetAnonymousState()
        {
            var anonymous = new ClaimsPrincipal(
                new ClaimsIdentity());

            return new AuthenticationState(anonymous);
        }

        private static ClaimsPrincipal SetClaimsPrincipal(string token)
        {
            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");

            return new ClaimsPrincipal(identity);
        }
        private static bool IsTokenExpired(string token)
        {
            var jwtToken = new JwtSecurityTokenHandler()
                .ReadJwtToken(token);

            return jwtToken.ValidTo <= DateTime.UtcNow;
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string token)
        {
            var jwtToken = new JwtSecurityTokenHandler()
                .ReadJwtToken(token);

            return jwtToken.Claims;
        }
    }
}
