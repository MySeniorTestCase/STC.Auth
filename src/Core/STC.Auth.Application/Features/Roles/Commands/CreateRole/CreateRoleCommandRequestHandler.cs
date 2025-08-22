using STC.Auth.Application.Features.Roles.Services;
using STC.Auth.Domain.Constants;
using STC.Auth.Domain.Roles;

namespace STC.Auth.Application.Features.Roles.Commands.CreateRole;

public class CreateRoleCommandRequestHandler(IRoleService roleService)
    : IRequestHandler<CreateRoleCommandRequest, IDataResponse<CreateRoleCommandResponse>>
{
    public async Task<IDataResponse<CreateRoleCommandResponse>> Handle(CreateRoleCommandRequest request,
        CancellationToken cancellationToken)
    {
        IDataResponse<Role> createResult = await roleService.CreateAsync(roleName: request.Name,
            claims: request.Claims,
            cancellationToken: cancellationToken);
        if (createResult.IsSuccess is false)
            return ResponseCreator.Error<CreateRoleCommandResponse>(response: createResult);

        return ResponseCreator.Success(message: Messages.RoleCreatedSuccessfully,
            data: new CreateRoleCommandResponse(Id: createResult.Data!.Id));
    }
}