using FluentValidation;
using MediatR;
using EFCoreShowcase.Common.Results;

namespace EFCoreShowcase.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .Select(e => e.ErrorMessage)
            .ToArray();

        if (failures.Any())
        {
            // Try to create a failed Result<T> if TResponse is Result<T>
            if (typeof(TResponse).IsGenericType &&
                typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var genericType = typeof(TResponse).GetGenericArguments()[0];
                var failureMethod = typeof(Result<>)
                    .MakeGenericType(genericType)
                    .GetMethod(nameof(Result<object>.Failure), new[] { typeof(string[]) });

                return (TResponse)failureMethod!.Invoke(null, new object[] { failures })!;
            }

            // Fallback to regular Result
            return (TResponse)Result.Failure(failures);
        }

        return await next();
    }
}
