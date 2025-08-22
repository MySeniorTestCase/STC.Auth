using MediatR;
using Microsoft.AspNetCore.Mvc;
using STC.Auth.Application.Features.Users.Commands.CreateUser;
using STC.Auth.Application.Features.Users.Queries.LoginUserWithCredentials;
using STC.Auth.Application.Features.Users.Queries.LoginUserWithRefreshToken;

namespace STC.Auth.WebAPI.ApiGroups;

public static class UsersExtensions
{
    public static IEndpointRouteBuilder MapUsersApi(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(prefix: "users").WithTags("Users");

        group.MapPost("/",
                async ([FromBody] CreateUserCommandRequest request, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(request, cancellationToken: cancellationToken);
                    return new ResponseGenerator(response: result);
                })
            .WithName("Add New User");

        group.MapPut("/login/credentials",
                async ([FromBody] LoginUserWithCredentialsQueryRequest request, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(request, cancellationToken: cancellationToken);
                    return new ResponseGenerator(response: result);
                })
            .AllowAnonymous()
            .WithName("Login User With Credentials");

        group.MapPut("/login/refresh-token",
                async ([FromBody] LoginUserWithRefreshTokenQueryRequest request, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(request, cancellationToken: cancellationToken);
                    return new ResponseGenerator(response: result);
                })
            .AllowAnonymous()
            .WithName("Login User With Refresh Token");

        return app;
    }
}