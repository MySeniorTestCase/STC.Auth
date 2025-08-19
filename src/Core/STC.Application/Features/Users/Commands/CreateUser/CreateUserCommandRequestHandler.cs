using STC.Application.Features.Users.Services;
using STC.Domain.Constants;
using STC.Domain.Users;

namespace STC.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandRequestHandler(IUserService userService, IUserTokenService userTokenService)
    : IRequestHandler<CreateUserCommandRequest, IDataResponse<CreateUserCommandResponse>>
{
    public async Task<IDataResponse<CreateUserCommandResponse>> Handle(CreateUserCommandRequest request,
        CancellationToken cancellationToken)
    {
        IDataResponse<User> createUserResult = await userService.CreateAsync(userName: request.UserName,
            password: request.Password,
            cancellationToken: cancellationToken);
        if (createUserResult.IsSuccess is false)
            return ResponseCreator.Error<CreateUserCommandResponse>(response: createUserResult);

        return ResponseCreator.Success(message: Messages.UserCreatedSuccessfully,
            new CreateUserCommandResponse(Id: createUserResult.Data!.Id));
    }
}