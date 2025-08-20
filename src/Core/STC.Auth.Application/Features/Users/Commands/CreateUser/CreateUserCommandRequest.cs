namespace STC.Auth.Application.Features.Users.Commands.CreateUser;

public record CreateUserCommandRequest : IRequest<IDataResponse<CreateUserCommandResponse>>
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
}