using MediatR;
using Microsoft.AspNetCore.Mvc;
using STC.Auth.Application.Features.Roles.Commands.CreateRole;

namespace STC.Auth.WebAPI.ApiGroups;

public static class RolesExtensions
{
    public static void MapRolesApi(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(prefix: "/api/roles").WithTags("Roles");

        group.MapPost("/roles",
                async ([FromBody] CreateRoleCommandRequest request, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(request, cancellationToken: cancellationToken);
                    return new ResponseGenerator(response: result);
                })
            .WithName("Add New Role");
    }
}