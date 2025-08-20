namespace STC.Auth.Application.Features.Users.Queries.LoginUserWithRefreshToken;

public class LoginUserWithRefreshTokenQueryRequestValidator : AbstractValidator<LoginUserWithRefreshTokenQueryRequest>
{
    public LoginUserWithRefreshTokenQueryRequestValidator()
    {
        RuleFor(x => x.RefreshToken).NotNull().NotEmpty();
    }
}