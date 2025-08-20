namespace STC.Auth.Application.BehaviorPipelines.Validation;

public class ValidationException(List<KeyValuePair<string, string>> validationErrors) : Exception
{
    public List<KeyValuePair<string, string>> ValidationErrors { get; init; } = validationErrors;
}