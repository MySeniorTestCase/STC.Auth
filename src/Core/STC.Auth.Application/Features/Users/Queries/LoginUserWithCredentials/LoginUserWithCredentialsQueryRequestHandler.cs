using Microsoft.Extensions.Logging;
using STC.Auth.Application.Features.Users.Queries.GenerateUserTokens;
using STC.Auth.Application.Features.Users.Services;
using STC.Auth.Domain.Constants;
using STC.Auth.Domain.Roles;
using STC.Auth.Domain.Users;

namespace STC.Auth.Application.Features.Users.Queries.LoginUserWithCredentials;

public class LoginUserWithCredentialsQueryRequestHandler(
    IMediator mediator,
    IUserService userService,
    ILogger<LoginUserWithCredentialsQueryRequestHandler> logger)
    : IRequestHandler<LoginUserWithCredentialsQueryRequest, IDataResponse<GenerateUserTokensQueryResponse>>
{
    public async Task<IDataResponse<GenerateUserTokensQueryResponse>> Handle(
        LoginUserWithCredentialsQueryRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(message: "A login request received with credentials.");

        IDataResponse<User> canLoginWithCredentialsResult =
            await userService.LoginWithCredentialsAsync(userName: request.Username, password: request.Password,
                cancellationToken: cancellationToken);
        if (canLoginWithCredentialsResult.IsSuccess is false)
        {
            logger.LogWarning(message: "A user login attempt failed with credentials. Error: {0}",
                canLoginWithCredentialsResult.Message);
            return ResponseCreator.Error<GenerateUserTokensQueryResponse>(
                message: Messages.UserNameOrPasswordIsIncorrect);
        }

        var generateTokensResult = await mediator.Send(
            new GenerateUserTokensQueryRequest(User: canLoginWithCredentialsResult.Data!),
            cancellationToken: cancellationToken);
        if (generateTokensResult.IsSuccess is false)
        {
            logger.LogError(message: "Token generation failed. Error: {0}", generateTokensResult.Message);
            return generateTokensResult;
        }

        logger.LogInformation(message: "A user logged in successfully with credentials.");

        return ResponseCreator.Success(message: Messages.UserLoggedInSuccessfully, data: generateTokensResult.Data!);
    }
}