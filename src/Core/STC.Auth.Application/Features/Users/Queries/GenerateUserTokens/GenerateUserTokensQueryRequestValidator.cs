namespace STC.Auth.Application.Features.Users.Queries.GenerateUserTokens;

public class GenerateUserTokensQueryRequestValidator : AbstractValidator<GenerateUserTokensQueryRequest>
{
    public GenerateUserTokensQueryRequestValidator()
    {
        RuleFor(x => x.User).NotNull();
    }
}