
using Cinemax.Application.Abstractions.Services;

namespace Cinemax.Application.Services
{
    public class EmailBodyRenderer : IEmailBodyRenderer
    {
        public string Render(
            string template,
            IReadOnlyDictionary<string, string> values)
        {
            var result = template;

            foreach (var value in values)
            {
                result = result.Replace(
                    $"{{{{{value.Key}}}}}",
                    value.Value);
            }

            return result;
        }
    }
}
