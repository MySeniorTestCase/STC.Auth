using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using STC.Application.Features.Users.Services;
using STC.Domain.Constants;
using STC.Domain.Users;
using STC.Shared.Utilities.Response;
using STC.Shared.Utilities.Response.Abstracts;

namespace STC.Infrastructure.Features.Users.Services;

public class CoreIdentityUserManager(UserManager<User> userManager, SignInManager<User> signInManager) : IUserService
{
    public async ValueTask<IDataResponse<User>> CreateAsync(string userName, string password,
        CancellationToken cancellationToken)
    {
        string normalizedUserName = userManager.NormalizeName(name: userName);

        bool isUserNameAlreadyExists = await userManager.Users.AnyAsync(
            predicate: _user => _user.NormalizedUserName == normalizedUserName,
            cancellationToken: cancellationToken);
        if (isUserNameAlreadyExists)
            return ResponseCreator.Error<User>(message: Messages.InvalidUserName);

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