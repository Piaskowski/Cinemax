using System.ComponentModel.DataAnnotations;

namespace Cinemax.Shared.Common.Annotations
{

    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is not DateTime date)
                return true;

            return date > DateTime.UtcNow;
        }
    }
}
