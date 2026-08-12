using Cinemax.Shared.Resources.Validation;
using FluentValidation;

namespace Cinemax.Application.Elements.Tickets.Commands
{
    public class EditTicketPriceCommandValidator : AbstractValidator<EditTicketPriceCommand>
    {
        public EditTicketPriceCommandValidator()
        {
            RuleFor(x => x.Request)
                .NotNull()
                .WithMessage(ValidationMessages.NotNull);

            RuleFor(x => x.Request.ScreeningType)
                .NotNull()
                .WithMessage(ValidationMessages.Required_ScreeningType);

            RuleFor(x => x.Request.TicketType)
                .NotNull()
                .WithMessage(ValidationMessages.Required_TicketType);

            RuleFor(x => x.Request.Price)
                .GreaterThan(0)
                .PrecisionScale(18, 2, true)
                .WithMessage(ValidationMessages.MinPrice);
        }
    }
}
