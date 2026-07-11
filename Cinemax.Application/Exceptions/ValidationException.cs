
namespace Cinemax.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public IReadOnlyList<string> Errors { get; }

        public ValidationException(IEnumerable<string> errors)
            : base("Wystąpiły błędy")
        {
            Errors = [.. errors];
        }
    }
}
