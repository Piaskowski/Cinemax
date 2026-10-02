
using Cinemax.Shared.Resources.Validation;

namespace Cinemax.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public IReadOnlyList<string> Errors { get; }

        public ValidationException(IEnumerable<string> errors)
            : base(ValidationMessages.Error_ErrorsOccured)
        {
            Errors = [.. errors];
        }
    }
}
