
using Microsoft.JSInterop;

namespace Cinemax.Client.Auth
{
    public class TokenStorage(IJSRuntime jsRuntime) : ITokenStorage
    {
        private const string _tokenKey = "authToken";
        private readonly IJSRuntime _jsRuntime = jsRuntime;
        public async Task<string?> GetTokenAsync()
        {
            return await _jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem",
                _tokenKey);
        }

        public async Task RemoveTokenAsync()
        {
            await _jsRuntime.InvokeVoidAsync(
                "localStorage.removeItem",
                _tokenKey);
        }

        public async Task SetTokenAsync(string token)
        {
            await _jsRuntime.InvokeVoidAsync(
                "localStorage.setItem",
                _tokenKey,
                token);
        }
    }
}
