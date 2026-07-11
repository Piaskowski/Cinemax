
namespace Cinemax.Shared.Contracts.Common
{
    public class ImportResult<T> where T : class
    {
        public List<T> Items { get; set; } = [];
        public List<string> Errors { get; set; } = [];
    }
}
