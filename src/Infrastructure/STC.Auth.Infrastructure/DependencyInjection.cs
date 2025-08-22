using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using STC.Auth.Application.Features.Roles.Services;
using STC.Auth.Application.Features.Seeds.Services;
using STC.Auth.Application.Features.Users.Services;
using STC.Auth.Infrastructure.Features.Roles.Services;
using STC.Auth.Infrastructure.Features.Seeds;
using STC.Auth.Infrastructure.Features.Users.Services;
using STC.Auth.Infrastructure.Features.Users.Services.Tokens;
using STC.Shared.Logger;

namespace STC.Auth.Infrastructure;

public static class DependencyInjection
{
    private static IServiceCollection UseJwt(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(config: configuration.GetRequiredSection(key: "Auth:JwtSettings"));

        IOptions<JwtSettings> jwtSettingsOptions =
            services.BuildServiceProvider().GetRequiredService<IOptions<JwtSettings>>();

        Shared.JwtAuthentication.DependencyInjection.AddJwtAuthenticationDependencies(services: services, options:
            jwtOptions =>
            {
                jwtOptions.SecurityKey = jwtSettingsOptions.Value.SecurityKey;
                jwtOptions.Audience = jwtSettingsOptions.Value.Audience;
                jwtOptions.Issuer = jwtSettingsOptions.Value.Issuer;
            });

        services.AddScoped<IUserTokenService, JwtUserTokenManager>();

        return services;
    }

    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services,
        IConfiguration configuration, ILoggingBuilder loggingBuilder)
    {
        services.AddScoped<ISeedDataService, SeedDataManager>();

        services.AddLoggerDependencies(loggingBuilder: loggingBuilder,
            options: _loggerOpt =>
            {
                var loggerSect = configuration.GetRequiredSection("Logger");
                var rabbitMqSect = loggerSect.GetRequiredSection("RabbitMq");

                _loggerOpt.ApplicationName = Assembly.GetExecutingAssembly().FullName!;
                _loggerOpt.ExchangeName = loggerSect["ExchangeName"]!;
                _loggerOpt.BatchPostingLimit = int.Parse(loggerSect["BatchPostingLimit"]!);
                _loggerOpt.RabbitMqOptions = new RabbitMqLoggerOptions()
                {
                    HostName = rabbitMqSect["HostName"]!,
                    Port = int.Parse(rabbitMqSect["Port"]!),
                    UserName = rabbitMqSect["UserName"]!,
                    Password = rabbitMqSect["Password"]!
                };
            });

        services.AddScoped<IUserService, CoreIdentityUserManager>();
        services.AddScoped<IRoleService, CoreIdentityRoleManager>();

        services.Configure<IdentityOptions>(opt =>
        {
            opt.Password.RequireDigit = false;
            opt.Password.RequireLowercase = false;
            opt.Password.RequireUppercase = false;
            opt.Password.RequireNonAlphanumeric = false;
        });

        UseJwt(services: services, configuration: configuration);

        return services;
    }
}