using MediatR;
using Microsoft.AspNetCore.Mvc;
using STC.Auth.Application.Features.Users.Commands.CreateUser;
using STC.Auth.Application.Features.Users.Queries.LoginUserWithCredentials;
using STC.Auth.Application.Features.Users.Queries.LoginUserWithRefreshToken;

namespace STC.Auth.WebAPI.ApiGroups;

public static class UsersExtensions
{
    public static void MapUsersApi(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(prefix: "/api/users").WithTags("Users");

        group.MapPost("/users",
                async ([FromBody] CreateUserCommandRequest request, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(request, cancellationToken: cancellationToken);
                    return new ResponseGenerator(response: result);
                })
            .WithName("Add New User");

        group.MapPost("/users/login-with-credentials",
                async ([FromBody] LoginUserWithCredentialsQueryRequest request, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(request, cancellationToken: cancellationToken);
                    return new ResponseGenerator(response: result);
                })
            .WithName("Login User With Credentials");

        group.MapPost("/users/login-with-refresh-token",
                async ([FromBody] LoginUserWithRefreshTokenQueryRequest request, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(request, cancellationToken: cancellationToken);
                    return new ResponseGenerator(response: result);
                })
            .WithName("Login User With Refresh Token");
    }
}