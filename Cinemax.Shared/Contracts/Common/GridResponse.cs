
namespace Cinemax.Shared.Contracts.Common
{
    public class GridResponse<TItem>
    {
        public List<TItem> Items { get; init; } = [];
        public int TotalCount { get; init; }
    }
}
