
using Cinemax.Shared.Resources.Validation;
using FluentValidation;

namespace Cinemax.Application.Elements.Movies.Commands
{
    public class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
    {
        public CreateMovieCommandValidator()
        {
            RuleFor(x => x.Request)
                .NotNull()
                .WithMessage(ValidationMessages.NotNull);

            RuleFor(x => x.Request.Title)
                .NotEmpty()
                .WithMessage(ValidationMessages.Required_Title);

            RuleFor(x => x.Request.Director)
                .NotEmpty()
                .WithMessage(ValidationMessages.Required_Director);

            RuleFor(x => x.Request.Description)
                .NotEmpty()
                .WithMessage(ValidationMessages.Required_Description);

            RuleFor(x => x.Request.DurationMinutes)
                .ExclusiveBetween(0, 1200)
                .WithMessage(ValidationMessages.MinDuration);

            RuleFor(x => x.Request.GenresIds)
                .NotEmpty()
                .WithMessage(ValidationMessages.NotEmpty_GenresList);
        }
    }
}
