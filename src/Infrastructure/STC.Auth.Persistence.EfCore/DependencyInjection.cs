using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using STC.Auth.Domain.Roles;
using STC.Auth.Domain.Users;

namespace STC.Auth.Persistence.EfCore;

public static class DependencyInjection
{
    public static IServiceCollection AddEfCorePersistenceDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddIdentityCore<User>().AddEntityFrameworkStores<AuthDbContext>();

        services.AddDbContext<AuthDbContext>(optionsAction: _act =>
            _act.UseNpgsql(connectionString: configuration.GetConnectionString(name: "Database")));

        return services;
    }
}