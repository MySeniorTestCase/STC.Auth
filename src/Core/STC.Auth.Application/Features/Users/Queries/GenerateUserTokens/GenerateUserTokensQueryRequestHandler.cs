using STC.Auth.Application.Features.Users.Services;
using STC.Auth.Domain.Constants;

namespace STC.Auth.Application.Features.Users.Queries.GenerateUserTokens;

public class
    GenerateUserTokensQueryRequestHandler(IUserTokenService userTokenService, IUserService userService)
    : IRequestHandler<GenerateUserTokensQueryRequest, IDataResponse<GenerateUserTokensQueryResponse>>
{
    public async Task<IDataResponse<GenerateUserTokensQueryResponse>> Handle(GenerateUserTokensQueryRequest request,
        CancellationToken cancellationToken)
    {
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
            return ResponseCreator.Error<GenerateUserTokensQueryResponse>(message: Messages.RefreshTokenCouldNotBeSaved);

        GenerateUserTokensQueryResponse response = new(
            AccessToken: new GenerateUserTokensQueryResponse.TokenDto(Token: accessTokenResult.Token,
                ExpiryDate: accessTokenResult.ExpiryDate),
            RefreshToken: new GenerateUserTokensQueryResponse.TokenDto(Token: refreshTokenResult.Token,
                ExpiryDate: refreshTokenResult.ExpiryDate));

        return ResponseCreator.Success(message: Messages.UserLoggedInSuccessfully, data: response);
    }
}