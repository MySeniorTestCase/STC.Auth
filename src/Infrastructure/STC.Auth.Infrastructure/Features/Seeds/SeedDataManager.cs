using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using STC.Auth.Application.Features.Roles.Commands.CreateRole;
using STC.Auth.Application.Features.Seeds.Services;
using STC.Auth.Application.Features.Users.Commands.CreateUser;
using STC.Auth.Domain.Roles;
using STC.Auth.Domain.Users;

namespace STC.Auth.Infrastructure.Features.Seeds;

public class SeedDataManager(IMediator mediator, UserManager<User> userManager, RoleManager<Role> roleManager)
    : ISeedDataService
{
    public async Task CheckAndAddSeedAsync(CancellationToken cancellationToken)
    {
        long roleCount = await roleManager.Roles.LongCountAsync(cancellationToken: cancellationToken);
        if (roleCount is 0)
        {
            var createModeratorResult = await mediator.Send(new CreateRoleCommandRequest(Name: "moderator"),
                cancellationToken: cancellationToken);
            if (createModeratorResult.IsSuccess is false)
                throw new InvalidOperationException(message: createModeratorResult.Message);

            var createAdminResult = await mediator.Send(new CreateRoleCommandRequest(Name: "admin"),
                cancellationToken: cancellationToken);
            if (createAdminResult.IsSuccess is false)
                throw new InvalidOperationException(message: createAdminResult.Message);
        }

        long userCount = await userManager.Users.LongCountAsync(cancellationToken: cancellationToken);
        if (userCount is 0)
        {
            ICollection<Role> roles = await roleManager.Roles.ToListAsync(cancellationToken: cancellationToken);

            var createAdminResult = await mediator.Send(
                new CreateUserCommandRequest(RoleId: roles.First(x => x.Name == "admin").Id,
                    UserName: "admin",
                    Password: "test1234"),
                cancellationToken: cancellationToken);
            if (createAdminResult.IsSuccess is false)
                throw new InvalidOperationException(message: createAdminResult.Message);

            var createModeratorResult = await mediator.Send(
                new CreateUserCommandRequest(RoleId: roles.First(x => x.Name == "moderator").Id,
                    UserName: "moderator",
                    Password: "test1234"),
                cancellationToken: cancellationToken);
            if (createAdminResult.IsSuccess is false)
                throw new InvalidOperationException(message: createModeratorResult.Message);
        }
    }
}