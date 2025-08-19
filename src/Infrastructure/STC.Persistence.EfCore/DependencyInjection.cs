using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace STC.Persistence.EfCore;

public static class DependencyInjection
{
    public static IServiceCollection AddEfCorePersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AuthDbContext>(optionsAction: _act =>
            _act.UseNpgsql(connectionString: configuration.GetConnectionString(name: "AuthDb")));
        
        return services;
    }
}