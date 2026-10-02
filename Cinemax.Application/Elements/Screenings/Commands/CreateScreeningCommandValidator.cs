
using Cinemax.Shared.Resources.Validation;
using FluentValidation;

namespace Cinemax.Application.Elements.Screenings.Commands
{
    public class CreateScreeningCommandValidator : AbstractValidator<CreateScreeningCommand>
    {
        public CreateScreeningCommandValidator()
        {
            RuleFor(x => x.Request)
                .NotNull()
                .WithMessage(ValidationMessages.NotNull);

            RuleFor(x => x.Request.MovieId)
                .NotEmpty()
                .WithMessage(ValidationMessages.Required_Movie);

            RuleFor(x => x.Request.CinemaHallId)
                .NotEmpty()
                .WithMessage(ValidationMessages.Required_CinemaHall);

            RuleFor(x => x.Request.ScreeningType)
                .NotNull()
                .WithMessage(ValidationMessages.Required_ScreeningType);

            RuleFor(x => x.Request.StartTime)
                .NotNull()
                .WithMessage(ValidationMessages.Required_Date);

            RuleFor(x => x.Request.StartTime)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage(ValidationMessages.FutureDate);
        }
    }
}
