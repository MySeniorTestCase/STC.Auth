using STC.Auth.Application.Features.Users.Queries.GenerateUserTokens;

namespace STC.Auth.Application.Features.Users.Queries.LoginUserWithRefreshToken;

public record LoginUserWithRefreshTokenQueryRequest(string RefreshToken)
    : IRequest<IDataResponse<GenerateUserTokensQueryResponse>>;