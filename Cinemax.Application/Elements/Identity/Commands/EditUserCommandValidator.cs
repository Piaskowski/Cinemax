using Cinemax.Shared.Resources.Validation;
using FluentValidation;

namespace Cinemax.Application.Elements.Identity.Commands
{
    public class EditUserCommandValidator : AbstractValidator<EditUserCommand>
    {
        public EditUserCommandValidator()
        {
            RuleFor(x => x.Request)
                .NotNull()
                .WithMessage(ValidationMessages.NotNull);

            // First name
            RuleFor(x => x.Request.FirstName)
                .NotEmpty()
                .WithMessage(ValidationMessages.Required_FirstName);

            RuleFor(x => x.Request.FirstName)
                .MaximumLength(100)
                .WithMessage(ValidationMessages.MaxLength_FirstName);

            RuleFor(x => x.Request.FirstName)
                .MinimumLength(2)
                .WithMessage(ValidationMessages.MinLength_FirstName);

            // Last name
            RuleFor(x => x.Request.LastName)
                .NotEmpty()
                .WithMessage(ValidationMessages.Required_LastName);

            RuleFor(x => x.Request.LastName)
                .MaximumLength(100)
                .WithMessage(ValidationMessages.MaxLength_LastName);

            RuleFor(x => x.Request.LastName)
                .MinimumLength(2)
                .WithMessage(ValidationMessages.MinLength_LastName);

            // Email
            RuleFor(x => x.Request.Email)
                .NotEmpty()
                .WithMessage(ValidationMessages.Required_Email);

            RuleFor(x => x.Request.Email)
                .EmailAddress()
                .WithMessage(ValidationMessages.EmailAddress);

            RuleFor(x => x.Request.Email)
                .MaximumLength(320)
                .WithMessage(ValidationMessages.MaxLength_Email);

            // Role
            RuleFor(x => x.Request.Email)
                .NotNull()
                .WithMessage(ValidationMessages.Required_Role);
        }
    }
}
