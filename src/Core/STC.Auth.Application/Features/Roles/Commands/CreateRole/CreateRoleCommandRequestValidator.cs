namespace STC.Auth.Application.Features.Roles.Commands.CreateRole;

public class CreateRoleCommandRequestValidator : AbstractValidator<CreateRoleCommandRequest>
{
    public CreateRoleCommandRequestValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.Claims).NotNull();
    }
}