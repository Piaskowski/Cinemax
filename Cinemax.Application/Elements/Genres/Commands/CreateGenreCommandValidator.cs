
using Cinemax.Shared.Resources.Validation;
using FluentValidation;

namespace Cinemax.Application.Elements.Genres.Commands
{
    public class CreateGenreCommandValidator : AbstractValidator<CreateGenreCommand>
    {
        public CreateGenreCommandValidator()
        {
            RuleFor(x => x.Request)
                .NotNull()
                .WithMessage(ValidationMessages.NotNull);

            RuleFor(x => x.Request.Name)
                .NotEmpty()
                .WithMessage(ValidationMessages.Required_Name);

            RuleFor(x => x.Request.Name)
                .MaximumLength(50)
                .WithMessage(ValidationMessages.MaxLength);
        }
    }
}
