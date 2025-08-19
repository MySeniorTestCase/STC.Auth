using STC.Domain.Roles;
using STC.Domain.Users;

namespace STC.Application.Features.Users.Services;

public interface IUserTokenService
{
    public ValueTask<(string Token, DateTime ExpiryDate)> GenerateAccessTokenAsync(User user, ICollection<Role> roles,
        CancellationToken cancellationToken = default);
    
    public ValueTask<(string Token, DateTime ExpiryDate)> GenerateRefreshTokenAsync(User user, CancellationToken cancellationToken = default);
}