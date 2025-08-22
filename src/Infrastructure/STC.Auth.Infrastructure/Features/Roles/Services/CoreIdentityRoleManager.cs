using Microsoft.AspNetCore.Identity;
using STC.Auth.Application.Features.Roles.Services;
using STC.Auth.Domain.Constants;
using STC.Auth.Domain.Roles;
using STC.Shared.Utilities.Response;
using STC.Shared.Utilities.Response.Abstracts;

namespace STC.Auth.Infrastructure.Features.Roles.Services;

public class CoreIdentityRoleManager(RoleManager<Role> roleManager) : IRoleService
{
    public async Task<IDataResponse<Role>> CreateAsync(string roleName, CancellationToken cancellationToken)
    {
        Role role = new Role()
        {
            Name = roleName
        };
        IdentityResult result = await roleManager.CreateAsync(role: role);
        if (result.Succeeded is false)
        {
            string errors = string.Join(", ", result.Errors.Select(_identityError => _identityError.Description));

            return ResponseCreator.Error<Role>(message: errors);
        }

        return ResponseCreator.Success(message: Messages.RoleCreatedSuccessfully, data: role);
    }
}