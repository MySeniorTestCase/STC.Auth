using STC.Auth.Domain.Roles;

namespace STC.Auth.Application.Features.Roles.Services;

public interface IRoleService
{
    Task<IDataResponse<Role>> CreateAsync(string roleName, CancellationToken cancellationToken);
}