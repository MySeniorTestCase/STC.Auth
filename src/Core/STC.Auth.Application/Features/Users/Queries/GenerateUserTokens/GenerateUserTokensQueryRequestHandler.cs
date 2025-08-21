using Microsoft.Extensions.Logging;
using STC.Auth.Application.Features.Users.Services;
using STC.Auth.Domain.Constants;

namespace STC.Auth.Application.Features.Users.Queries.GenerateUserTokens;

public class
    GenerateUserTokensQueryRequestHandler(
        IUserTokenService userTokenService,
        IUserService userService,
        ILogger<GenerateUserTokensQueryRequestHandler> logger)
    : IRequestHandler<GenerateUserTokensQueryRequest, IDataResponse<GenerateUserTokensQueryResponse>>
{
    public async Task<IDataResponse<GenerateUserTokensQueryResponse>> Handle(GenerateUserTokensQueryRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(message: "User tokens are generating.");

        var accessTokenResult = await userTokenService.GenerateAccessTokenAsync(user: request.User,
            roles: [],
            cancellationToken: cancellationToken);

        var refreshTokenResult = await userTokenService.GenerateRefreshTokenAsync(user: request.User,
            cancellationToken: cancellationToken);

        IResponse saveRefreshTokenToUserResult = await userService.SetRefreshTokenAsync(user: request.User,
            refreshToken: refreshTokenResult.Token,
            expiryDate: refreshTokenResult.ExpiryDate,
            cancellationToken: cancellationToken);
        if (saveRefreshTokenToUserResult.IsSuccess is false)
        {
            logger.LogError(message: "Refresh token could not be saved. Error: {0}",
                saveRefreshTokenToUserResult.Message);

            return ResponseCreator.Error<GenerateUserTokensQueryResponse>(
                message: Messages.RefreshTokenCouldNotBeSaved);
        }

        GenerateUserTokensQueryResponse response = new(
            AccessToken: new GenerateUserTokensQueryResponse.TokenDto(Token: accessTokenResult.Token,
                ExpiryDate: accessTokenResult.ExpiryDate),
            RefreshToken: new GenerateUserTokensQueryResponse.TokenDto(Token: refreshTokenResult.Token,
                ExpiryDate: refreshTokenResult.ExpiryDate));

        logger.LogInformation(message: "User tokens are generated successfully.");

        return ResponseCreator.Success(message: Messages.UserLoggedInSuccessfully, data: response);
    }
}