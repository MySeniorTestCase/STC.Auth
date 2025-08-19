using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using STC.Application.Features.Users.Services;
using STC.Domain.Roles;
using STC.Domain.Users;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace STC.Infrastructure.Features.Users.Services.Tokens;

public class JwtUserTokenManager(IOptions<JwtSettings> jwtSettingsOptions) : IUserTokenService
{
    public async ValueTask<(string Token, DateTime ExpiryDate)> GenerateAccessTokenAsync(User user,
        ICollection<Role> roles, CancellationToken cancellationToken = default)
    {
        ICollection<Claim> claims =
        [
            new Claim(type: JwtRegisteredClaimNames.UniqueName, user.Id),
            new Claim(type: JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        ];

        if (string.IsNullOrEmpty(user.Email) is false)
            claims.Add(new Claim(type: JwtRegisteredClaimNames.Email, user.Email));

        if (string.IsNullOrEmpty(user.UserName) is false)
            claims.Add(new Claim(type: JwtRegisteredClaimNames.PreferredUsername, user.UserName));

        foreach (Role role in roles)
        {
            string? roleName = role.NormalizedName ?? role.Name;
            if (string.IsNullOrEmpty(roleName))
                continue;

            claims.Add(new Claim(type: JwtRegisteredClaimNames.MiddleName, value: roleName));
        }

        var symmetricSecurityKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettingsOptions.Value.SecurityKey));

        var signingCredentials = new SigningCredentials(symmetricSecurityKey, algorithm: SecurityAlgorithms.HmacSha256);

        DateTime expiryDate = DateTime.UtcNow.AddMinutes(jwtSettingsOptions.Value.TokenExpiryTimeAsMinute);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: jwtSettingsOptions.Value.Issuer,
            audience: jwtSettingsOptions.Value.Audience,
            claims: claims,
            expires: expiryDate,
            signingCredentials: signingCredentials);

        string token = new JwtSecurityTokenHandler().WriteToken(token: jwtSecurityToken);

        return await Task.FromResult((Token: token, ExpiryDate: expiryDate));
    }

    public async ValueTask<(string Token, DateTime ExpiryDate)> GenerateRefreshTokenAsync(User user,
        CancellationToken cancellationToken = default)
    {
        var symmetricSecurityKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettingsOptions.Value.SecurityKey));

        var signingCredentials = new SigningCredentials(symmetricSecurityKey, algorithm: SecurityAlgorithms.HmacSha256);

        DateTime expiryDate = DateTime.UtcNow.AddMinutes(jwtSettingsOptions.Value.TokenExpiryTimeAsMinute);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: jwtSettingsOptions.Value.Issuer,
            audience: jwtSettingsOptions.Value.Audience,
            claims: [new Claim(type: JwtRegisteredClaimNames.UniqueName, user.Id)],
            expires: expiryDate,
            signingCredentials: signingCredentials);

        string token = new JwtSecurityTokenHandler().WriteToken(token: jwtSecurityToken);

        return await Task.FromResult((Token: token,
            ExpiryDate: DateTime.UtcNow.AddMinutes(jwtSettingsOptions.Value.RefreshTokenExpiryTimeAsMinute)));
    }
}