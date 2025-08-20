namespace STC.Auth.Application.Features.Users.Queries.LoginUser;

public record LoginUserQueryResponse(
    LoginUserQueryResponse.TokenDto AccessToken,
    LoginUserQueryResponse.TokenDto RefreshToken)
{
    public record TokenDto(string Token, DateTime ExpiryDate);
}