using STC.Auth.Application.Features.Users.Queries.GenerateUserTokens;
using STC.Auth.Application.Features.Users.Services;
using STC.Auth.Domain.Constants;
using STC.Auth.Domain.Users;

namespace STC.Auth.Application.Features.Users.Queries.LoginUserWithCredentials;

public class LoginUserWithCredentialsQueryRequestHandler(IMediator mediator, IUserService userService)
    : IRequestHandler<LoginUserWithCredentialsQueryRequest, IDataResponse<GenerateUserTokensQueryResponse>>
{
    public async Task<IDataResponse<GenerateUserTokensQueryResponse>> Handle(
        LoginUserWithCredentialsQueryRequest request,
        CancellationToken cancellationToken)
    {
        IDataResponse<User> canLoginWithCredentialsResult =
            await userService.LoginAsync(userName: request.Username, password: request.Password,
                cancellationToken: cancellationToken);
        if (canLoginWithCredentialsResult.IsSuccess is false)
            return ResponseCreator.Error<GenerateUserTokensQueryResponse>(
                message: Messages.UserNameOrPasswordIsIncorrect);

        var generateTokensResult = await mediator.Send(
            new GenerateUserTokensQueryRequest(User: canLoginWithCredentialsResult.Data!),
            cancellationToken: cancellationToken);
        if (generateTokensResult.IsSuccess is false)
            return generateTokensResult;

        return ResponseCreator.Success(message: Messages.UserLoggedInSuccessfully, data: generateTokensResult.Data!);
    }
}