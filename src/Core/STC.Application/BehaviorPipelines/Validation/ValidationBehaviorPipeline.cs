namespace STC.Application.BehaviorPipelines.Validation;

public sealed class ValidationBehaviorPipeline<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationFailures = await Task.WhenAll(
            tasks: validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

        var errors = validationFailures
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .Select(validationFailure =>
                new KeyValuePair<string, string>(validationFailure.PropertyName, validationFailure.ErrorMessage))
            .ToList();

        if (errors.Count != 0)
            throw new ValidationException(validationErrors: errors);

        return await next(t: cancellationToken);
    }
}