namespace Cinemax.Client.Configuration
{
    public class LocalizationOptions
    {
        public string DefaultCulture { get; set; } = default!;
        public List<CultureOption> SupportedCultures { get; set; } = [];
    }
}
