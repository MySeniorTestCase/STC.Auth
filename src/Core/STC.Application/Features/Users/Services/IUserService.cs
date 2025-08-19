using STC.Domain.Users;

namespace STC.Application.Features.Users.Services;

public interface IUserService
{
    ValueTask<IDataResponse<User>> CreateAsync(string userName, string password, CancellationToken cancellationToken);

    ValueTask<IDataResponse<User?>> GetByUserNameAsync(string userName, CancellationToken cancellationToken);

    ValueTask<IResponse> SetRefreshTokenAsync(User user, string refreshToken, DateTime expiryDate,
        CancellationToken cancellationToken);
}