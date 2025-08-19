using STC.Application.Features.Users.Services;
using STC.Domain.Constants;
using STC.Domain.Users;

namespace STC.Application.Features.Users.Queries.LoginUser;

public class LoginUserQueryRequestHandler(IUserTokenService userTokenService, IUserService userService)
    : IRequestHandler<LoginUserQueryRequest, IDataResponse<LoginUserQueryResponse>>
{
    public async Task<IDataResponse<LoginUserQueryResponse>> Handle(LoginUserQueryRequest request,
        CancellationToken cancellationToken)
    {
        IDataResponse<User?> userResult =
            await userService.GetByUserNameAsync(userName: request.Username, cancellationToken: cancellationToken);
        if (userResult.IsSuccess is false)
            return ResponseCreator.Error<LoginUserQueryResponse>(message: Messages.UserNameOrPasswordIsIncorrect);

        var accessTokenResult = await userTokenService.GenerateAccessTokenAsync(user: userResult.Data!,
            roles: [],
            cancellationToken: cancellationToken);

        var refreshTokenResult = await userTokenService.GenerateRefreshTokenAsync(user: userResult.Data!,
            cancellationToken: cancellationToken);

        IResponse saveRefreshTokenToUserResult = await userService.SetRefreshTokenAsync(user: userResult.Data!,
            refreshToken: refreshTokenResult.Token,
            expiryDate: refreshTokenResult.ExpiryDate,
            cancellationToken: cancellationToken);
        if (saveRefreshTokenToUserResult.IsSuccess is false)
            return ResponseCreator.Error<LoginUserQueryResponse>(message: Messages.RefreshTokenCouldNotBeSaved);

        LoginUserQueryResponse response = new(
            AccessToken: new LoginUserQueryResponse.TokenDto(Token: accessTokenResult.Token,
                ExpiryDate: accessTokenResult.ExpiryDate),
            RefreshToken: new LoginUserQueryResponse.TokenDto(Token: refreshTokenResult.Token,
                ExpiryDate: refreshTokenResult.ExpiryDate));

        return ResponseCreator.Success(message: Messages.UserLoggedInSuccessfully, data: response);
    }
}