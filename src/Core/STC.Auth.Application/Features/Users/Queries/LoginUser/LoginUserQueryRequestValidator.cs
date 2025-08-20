namespace STC.Auth.Application.Features.Users.Queries.LoginUser;

public class LoginUserQueryRequestValidator : AbstractValidator<LoginUserQueryRequest>
{
    public LoginUserQueryRequestValidator()
    {
        RuleFor(x => x.Username).NotNull().NotEmpty();
        RuleFor(x => x.Password).NotNull().NotEmpty();
    }
}