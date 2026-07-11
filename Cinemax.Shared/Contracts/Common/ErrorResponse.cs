
namespace Cinemax.Shared.Contracts.Common
{
    public class ErrorResponse : GeneralResponse
    {
        public List<string> Errors { get; init; } = [];
    }
}
