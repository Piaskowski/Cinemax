
namespace Cinemax.Application.Abstractions.Services
{
    public interface IEmailBodyRenderer
    {
        string Render(
            string template,
            IReadOnlyDictionary<string, string> values);
    }
}
