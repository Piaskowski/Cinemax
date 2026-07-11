namespace Cinemax.Client.Auth
{
    public interface ITokenStorage
    {
        Task<string?> GetTokenAsync();
        Task SetTokenAsync(string token);
        Task RemoveTokenAsync();
    }
}
