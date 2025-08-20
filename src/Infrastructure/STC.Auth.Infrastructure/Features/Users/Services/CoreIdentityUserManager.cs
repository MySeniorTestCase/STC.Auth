using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using STC.Auth.Application.Features.Users.Services;
using STC.Auth.Domain.Constants;
using STC.Auth.Domain.Users;
using STC.Shared.Utilities.Response;
using STC.Shared.Utilities.Response.Abstracts;

namespace STC.Auth.Infrastructure.Features.Users.Services;

public class CoreIdentityUserManager(UserManager<User> userManager) : IUserService
{
    public async ValueTask<IDataResponse<User>> CreateAsync(string userName, string password,
        CancellationToken cancellationToken)
    {
        User user = new User()
        {
            UserName = userName,
        };

        IdentityResult result = await userManager.CreateAsync(user: user, password: password);
        if (result.Succeeded is false)
        {
            string errorMessage = string.Join(", ", result.Errors.Select(_error => _error.Description));

            return ResponseCreator.Error<User>(message: errorMessage);
        }

        return ResponseCreator.Success(message: Messages.UserCreatedSuccessfully, data: user);
    }

    public async ValueTask<IDataResponse<User?>> GetByUserNameAsync(string userName,
        CancellationToken cancellationToken)
    {
        User? user = await userManager.FindByNameAsync(userName: userName);

        return user is null
            ? ResponseCreator.Error<User?>(message: Messages.UserIsNotFound, data: null)
            : ResponseCreator.Success<User?>(message: string.Empty, data: user);
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
}