namespace STC.Auth.Application.Features.Roles.Commands.CreateRole;

public record CreateRoleCommandRequest(string Name, string[] Claims) : IRequest<IDataResponse<CreateRoleCommandResponse>>;