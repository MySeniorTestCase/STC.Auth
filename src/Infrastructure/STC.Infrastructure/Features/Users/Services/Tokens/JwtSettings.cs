namespace STC.Infrastructure.Features.Users.Services.Tokens;

public record JwtSettings
{
    public string SecurityKey { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public int TokenExpiryTimeAsMinute { get; init; }
    public int RefreshTokenExpiryTimeAsMinute { get; init; }
}