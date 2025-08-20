using STC.Auth.Application.Features.Users.Services;
using STC.Auth.Domain.Constants;
using STC.Auth.Domain.Users;

namespace STC.Auth.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandRequestHandler(IUserService userService)
    : IRequestHandler<CreateUserCommandRequest, IDataResponse<CreateUserCommandResponse>>
{
    public async Task<IDataResponse<CreateUserCommandResponse>> Handle(CreateUserCommandRequest request,
        CancellationToken cancellationToken)
    {
        IDataResponse<User> createUserResult = await userService.CreateAsync(userName: request.Username,
            password: request.Password,
            cancellationToken: cancellationToken);
        if (createUserResult.IsSuccess is false)
            return ResponseCreator.Error<CreateUserCommandResponse>(response: createUserResult);

        return ResponseCreator.Success(message: Messages.UserCreatedSuccessfully,
            new CreateUserCommandResponse(Id: createUserResult.Data!.Id));
    }
}