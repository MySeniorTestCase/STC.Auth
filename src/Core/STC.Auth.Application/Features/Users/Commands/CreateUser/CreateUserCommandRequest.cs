namespace STC.Auth.Application.Features.Users.Commands.CreateUser;

public record CreateUserCommandRequest(string RoleId, string UserName, string Password)
    : IRequest<IDataResponse<CreateUserCommandResponse>>;