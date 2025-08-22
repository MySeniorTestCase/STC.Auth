using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STC.Auth.Application.Features.Users.Services;
using STC.Auth.Domain.Constants;
using STC.Auth.Domain.Roles;
using STC.Auth.Domain.Users;
using STC.Shared.Utilities.Response;
using STC.Shared.Utilities.Response.Abstracts;

namespace STC.Auth.Infrastructure.Features.Users.Services;

public class CoreIdentityUserManager(
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    ILogger<CoreIdentityUserManager> logger) : IUserService
{
    public async ValueTask<IDataResponse<User>> CreateAsync(string roleId, string userName, string password,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(message: "User creation process has been starting.");

        Role? role = await roleManager.FindByIdAsync(roleId: roleId);
        if (role is null)
        {
            logger.LogWarning(message: "Role not found.");
            return ResponseCreator.Error<User>(message: Messages.RoleIsNotFound);
        }

        User user = new User()
        {
            UserName = userName,
        };

        IdentityResult result = await userManager.CreateAsync(user: user, password: password);
        if (result.Succeeded is false)
        {
            string errorMessage = string.Join(", ", result.Errors.Select(_error => _error.Description));

            logger.LogWarning(message: "A user creation failed. Error: {0}", errorMessage);

            return ResponseCreator.Error<User>(message: errorMessage);
        }

        var addRoleToUserResult = await userManager.AddToRoleAsync(user: user, role: role.Name!);
        if (addRoleToUserResult.Succeeded is false)
        {
            string errorMessage = string.Join(", ", result.Errors.Select(_error => _error.Description));

            logger.LogWarning(message: "Role assignment to user failed. Error: {0}", errorMessage);

            return ResponseCreator.Error<User>(message: errorMessage);
        }

        logger.LogInformation(message: "A user created successfully.");

        return ResponseCreator.Success(message: Messages.UserCreatedSuccessfully, data: user);
    }

    public async ValueTask<IDataResponse<User>> LoginWithCredentialsAsync(string userName, string password,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(message: "Login process with credentials has been starting.");

        User? user = await userManager.FindByNameAsync(userName: userName);
        if (user is null)
        {
            logger.LogWarning(message: "User not found for login.");
            return ResponseCreator.Error<User>(message: Messages.UserIsNotFound);
        }

        bool isPasswordInvalid = await userManager.CheckPasswordAsync(user: user, password: password) is false;
        if (isPasswordInvalid)
        {
            logger.LogWarning(message: "Password is invalid for user.");
            return ResponseCreator.Error<User>(message: Messages.InvalidPassword);
        }

        logger.LogInformation(message: "Login process with credentials has been completed successfully.");

        return ResponseCreator.Success<User>(message: string.Empty, data: user);
    }

    public async ValueTask<IDataResponse<User>> LoginWithRefreshTokenAsync(string refreshToken,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(message: "Login process with credentials has been starting.");

        User? user = await userManager.Users.FirstOrDefaultAsync(predicate: _user => _user.RefreshToken == refreshToken,
            cancellationToken: cancellationToken);
        if (user is null)
        {
            logger.LogWarning(message: "User not found for login.");
            return ResponseCreator.Error<User>(message: Messages.UserIsNotFound);
        }

        bool isRefreshTokenExpired = user.IsRefreshTokenValid() is false;
        if (isRefreshTokenExpired)
        {
            logger.LogWarning(message: "Refresh token is expired for user.");
            return ResponseCreator.Error<User>(message: Messages.TheRefreshTokenIsExpired);
        }

        logger.LogInformation(message: "Login process with credentials has been completed successfully.");

        return ResponseCreator.Success<User>(message: string.Empty, data: user);
    }

    public async ValueTask<IResponse> SetRefreshTokenAsync(User user, string refreshToken, DateTime expiryDate,
        CancellationToken cancellationToken)
    {
        user.SetRefreshToken(refreshToken: refreshToken, expiryDate: expiryDate);

        IdentityResult result = await userManager.UpdateAsync(user: user);

        return result.Succeeded
            ? ResponseCreator.Success(message: string.Empty)
            : ResponseCreator.Error(message: string.Join(", ", result.Errors.Select(_error => _error.Description)));
    }

    public async ValueTask<IDataResponse<Role[]>> GetRolesByUserAsync(User user, CancellationToken cancellationToken)
    {
        logger.LogInformation(message: "Fetch roles for user process has been starting.");

        ICollection<string> roleNames = await userManager.GetRolesAsync(user: user);
        if (roleNames.Count is 0)
        {
            logger.LogWarning(message: "User roles are not found.");
            return ResponseCreator.Success<Role[]>(message: Messages.TheUserHasNotRoles, data: []);
        }

        ICollection<Role> roles = await roleManager.Roles.Where(predicate: _role => roleNames.Contains(_role.Name!))
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
        if (roles.Count is 0)
        {
            logger.LogWarning(message: "User roles are not found.");
            return ResponseCreator.Error<Role[]>(message: Messages.SomeRolesAreNotFound, data: []);
        }

        logger.LogInformation(message: "User roles fetched successfully.");

        return ResponseCreator.Success(message: string.Empty, data: roles.ToArray());
    }
}