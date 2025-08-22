using Microsoft.Extensions.Logging;
using STC.Auth.Application.Features.Users.Services;
using STC.Auth.Domain.Constants;
using STC.Auth.Domain.Users;

namespace STC.Auth.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandRequestHandler(IUserService userService, ILogger<CreateUserCommandRequestHandler> logger)
    : IRequestHandler<CreateUserCommandRequest, IDataResponse<CreateUserCommandResponse>>
{
    public async Task<IDataResponse<CreateUserCommandResponse>> Handle(CreateUserCommandRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(message: "A user creation request has been received.");

        IDataResponse<User> createUserResult = await userService.CreateAsync(roleId: request.RoleId,
            userName: request.UserName,
            password: request.Password,
            cancellationToken: cancellationToken);
        if (createUserResult.IsSuccess is false)
        {
            logger.LogWarning(message: "User creation failed. Error: {0}", createUserResult.Message);
            return ResponseCreator.Error<CreateUserCommandResponse>(response: createUserResult);
        }

        logger.LogInformation(message: "A user created successfully.");

        return ResponseCreator.Success(message: Messages.UserCreatedSuccessfully,
            new CreateUserCommandResponse(Id: createUserResult.Data!.Id));
    }
}