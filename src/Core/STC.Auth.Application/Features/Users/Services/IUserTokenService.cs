using STC.Auth.Domain.Roles;
using STC.Auth.Domain.Users;

namespace STC.Auth.Application.Features.Users.Services;

public interface IUserTokenService
{
    public ValueTask<(string Token, DateTime ExpiryDate)> GenerateAccessTokenAsync(User user, ICollection<Role> roles,
        CancellationToken cancellationToken = default);
    
    public ValueTask<(string Token, DateTime ExpiryDate)> GenerateRefreshTokenAsync(User user, CancellationToken cancellationToken = default);
}