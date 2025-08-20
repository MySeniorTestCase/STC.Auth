using STC.Auth.Application.Features.Users.Queries.GenerateUserTokens;

namespace STC.Auth.Application.Features.Users.Queries.LoginUserWithCredentials;

public record LoginUserWithCredentialsQueryRequest(string Username, string Password) : IRequest<IDataResponse<GenerateUserTokensQueryResponse>>;