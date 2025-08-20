using STC.Auth.Domain.Users;

namespace STC.Auth.Application.Features.Users.Services;

public interface IUserService
{
    ValueTask<IDataResponse<User>> CreateAsync(string userName, string password, CancellationToken cancellationToken);

    ValueTask<IDataResponse<User>> LoginAsync(string userName, string password, CancellationToken cancellationToken);
    ValueTask<IDataResponse<User>> LoginWithRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);

    ValueTask<IResponse> SetRefreshTokenAsync(User user, string refreshToken, DateTime expiryDate,
        CancellationToken cancellationToken);
}