using Cinemax.Shared.Resources.Validation;
using FluentValidation;

namespace Cinemax.Application.Elements.CinemaHalls.Commands
{
    public class CreateCinemaHallCommandValidator : AbstractValidator<CreateCinemaHallCommand>
    {
        public CreateCinemaHallCommandValidator()
        {
            RuleFor(x => x.Request)
                .NotNull()
                .WithMessage(ValidationMessages.NotNull);

            RuleFor(x => x.Request.Number)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.MinValue_CinemaHall);

            RuleFor(x => x.Request.Type)
                .NotNull()
                .WithMessage(ValidationMessages.Required_CinemaHallType);
        }
    }
}
