using Microsoft.Extensions.Logging;
using STC.Auth.Application.Features.Users.Queries.GenerateUserTokens;
using STC.Auth.Application.Features.Users.Services;
using STC.Auth.Domain.Constants;

namespace STC.Auth.Application.Features.Users.Queries.LoginUserWithRefreshToken;

public class
    LoginUserWithRefreshTokenQueryRequestHandler(
        IUserService userService,
        IMediator mediator,
        ILogger<LoginUserWithRefreshTokenQueryRequestHandler> logger)
    : IRequestHandler<LoginUserWithRefreshTokenQueryRequest, IDataResponse<GenerateUserTokensQueryResponse>>
{
    public async Task<IDataResponse<GenerateUserTokensQueryResponse>> Handle(
        LoginUserWithRefreshTokenQueryRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(message: "A login request received with refresh token.");

        var canLoginWithRefreshToken = await userService.LoginWithRefreshTokenAsync(refreshToken: request.RefreshToken,
            cancellationToken: cancellationToken);
        if (canLoginWithRefreshToken.IsSuccess is false)
        {
            logger.LogWarning(message: "A user login attempt failed with refresh token. Error: {0}",
                canLoginWithRefreshToken.Message);
            return ResponseCreator.Error<GenerateUserTokensQueryResponse>(message: Messages.InvalidRefreshToken);
        }

        var generateTokensResult = await mediator.Send(
            new GenerateUserTokensQueryRequest(User: canLoginWithRefreshToken.Data!),
            cancellationToken: cancellationToken);
        if (generateTokensResult.IsSuccess is false)
        {
            logger.LogError(message: "Token generation failed. Error: {0}", generateTokensResult.Message);
            return generateTokensResult;
        }

        logger.LogInformation(message: "A user logged in successfully with refresh token.");

        return ResponseCreator.Success(message: Messages.UserLoggedInSuccessfully, data: generateTokensResult.Data!);
    }
}