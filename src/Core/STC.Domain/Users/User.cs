using Microsoft.AspNetCore.Identity;

namespace STC.Domain.Users;

public class User : IdentityUser
{
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiryDate { get; private set; }
    public virtual ICollection<UserRole> UserRoles { get; set; } = [];

    public void SetRefreshToken(string refreshToken, DateTime expiryDate)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiryDate = expiryDate;
    }
}