using Cinemax.Application.Exceptions;
using FluentValidation;
using MediatR;

namespace Cinemax.Application.Common
{
    public class ValidationBehavior<TRequest, TResponse>(
        IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {
            if (!_validators.Any())
            {
                return await next(ct);
            }

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, ct))
            );

            var errors = validationResults
                .SelectMany(r => r.Errors)
                .Where(e => e is not null)
                .Select(e => e.ErrorMessage)
                .ToArray();

            if (errors.Length != 0)
            {
                throw new Exceptions.ValidationException(errors);
            }

            return await next();
        }
    }
}
