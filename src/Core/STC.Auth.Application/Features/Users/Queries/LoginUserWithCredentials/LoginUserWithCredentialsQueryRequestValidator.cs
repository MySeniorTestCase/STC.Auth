namespace STC.Auth.Application.Features.Users.Queries.LoginUserWithCredentials;

public class LoginUserWithCredentialsQueryRequestValidator : AbstractValidator<LoginUserWithCredentialsQueryRequest>
{
    public LoginUserWithCredentialsQueryRequestValidator()
    {
        RuleFor(x => x.Username).NotNull().NotEmpty();
        RuleFor(x => x.Password).NotNull().NotEmpty();
    }
}