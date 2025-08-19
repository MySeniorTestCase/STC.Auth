namespace STC.Application.Features.Users.Commands.CreateUser;

public record CreateUserCommandRequest(string UserName, string Password) : IRequest<IDataResponse<CreateUserCommandResponse>>;