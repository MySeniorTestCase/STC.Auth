using STC.Auth.Domain.Users;

namespace STC.Auth.Application.Features.Users.Queries.GenerateUserTokens;

public record GenerateUserTokensQueryRequest(User User) : IRequest<IDataResponse<GenerateUserTokensQueryResponse>>;