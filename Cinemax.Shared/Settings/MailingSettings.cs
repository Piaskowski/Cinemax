
namespace Cinemax.Shared.Settings
{
    public class MailingSettings
    {
        public string Host { get; set; } = default!;
        public int Port { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
