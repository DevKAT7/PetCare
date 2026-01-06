using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ValidationException = PetCare.Application.Exceptions.ValidationException;

namespace PetCare.Application.Behaviors
{
    /// <summary>
    /// Pipeline Behavior wykonujący walidację przed Handlerem
    /// </summary>
    /// <typeparam name="TRequest">Typ żądania (Command/Query)</typeparam>
    /// <typeparam name="TResponse">Typ odpowiedzi</typeparam>
    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

        public ValidationBehavior(
            IEnumerable<IValidator<TRequest>> validators,
            ILogger<ValidationBehavior<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // Jeśli nie ma walidatorów - przejdź dalej
            if (!_validators.Any())
            {
                return await next();
            }

            var requestName = typeof(TRequest).Name;
            _logger.LogInformation("Validation {RequestName}", requestName);

            // Utwórz kontekst walidacji
            var context = new ValidationContext<TRequest>(request);

            // Wykonaj wszystkie walidatory
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );

            // Zbierz błędy
            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();

            // Jeśli są błędy - rzuć wyjątek
            if (failures.Any())
            {
                _logger.LogWarning(
                    "Validation {RequestName} failed. Errors: {@Errors}",
                    requestName,
                    failures.Select(f => new { f.PropertyName, f.ErrorMessage })
                );

                throw new ValidationException(failures);
            }

            _logger.LogInformation("Validation {RequestName} successful", requestName);

            // Walidacja OK - przejdź do Handlera
            return await next();
        }
    }
}
