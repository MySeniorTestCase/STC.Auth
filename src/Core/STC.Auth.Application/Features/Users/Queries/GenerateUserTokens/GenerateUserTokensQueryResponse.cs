namespace STC.Auth.Application.Features.Users.Queries.GenerateUserTokens;

public record GenerateUserTokensQueryResponse(
    GenerateUserTokensQueryResponse.TokenDto AccessToken,
    GenerateUserTokensQueryResponse.TokenDto RefreshToken)
{
    public record TokenDto(string Token, DateTime ExpiryDate);
}