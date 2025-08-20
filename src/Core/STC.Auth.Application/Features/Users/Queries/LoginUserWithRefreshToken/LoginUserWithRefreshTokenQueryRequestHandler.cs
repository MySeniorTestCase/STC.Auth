using STC.Auth.Application.Features.Users.Queries.GenerateUserTokens;
using STC.Auth.Application.Features.Users.Services;
using STC.Auth.Domain.Constants;

namespace STC.Auth.Application.Features.Users.Queries.LoginUserWithRefreshToken;

public class
    LoginUserWithRefreshTokenQueryRequestHandler(IUserService userService, IMediator mediator)
    : IRequestHandler<LoginUserWithRefreshTokenQueryRequest, IDataResponse<GenerateUserTokensQueryResponse>>
{
    public async Task<IDataResponse<GenerateUserTokensQueryResponse>> Handle(
        LoginUserWithRefreshTokenQueryRequest request,
        CancellationToken cancellationToken)
    {
        var canLoginWithRefreshToken = await userService.LoginWithRefreshTokenAsync(refreshToken: request.RefreshToken,
            cancellationToken: cancellationToken);
        if (canLoginWithRefreshToken.IsSuccess is false)
            return ResponseCreator.Error<GenerateUserTokensQueryResponse>(message: Messages.InvalidRefreshToken);

        var generateTokensResult = await mediator.Send(new GenerateUserTokensQueryRequest(User: canLoginWithRefreshToken.Data!),
            cancellationToken: cancellationToken);
        if (generateTokensResult.IsSuccess is false)
            return generateTokensResult;

        return ResponseCreator.Success(message: Messages.UserLoggedInSuccessfully, data: generateTokensResult.Data!);
    }
}