namespace STC.Auth.Application.Features.Users.Queries.LoginUser;

public record LoginUserQueryRequest(string Username, string Password) : IRequest<IDataResponse<LoginUserQueryResponse>>;