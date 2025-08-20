using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using STC.Auth.Application.Features.Users.Services;
using STC.Auth.Infrastructure.Features.Users.Services;
using STC.Auth.Infrastructure.Features.Users.Services.Tokens;

namespace STC.Auth.Infrastructure;

public static class DependencyInjection
{
    private static IServiceCollection UseJwt(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(config: configuration.GetSection(key: "Auth:JwtSettings") ??
                                                throw new ArgumentNullException(
                                                    message: "Auth:JwtSettings section is missing in configuration.",
                                                    innerException: null));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
        {
            IOptions<JwtSettings> jwtSettingsOptions =
                services.BuildServiceProvider().GetRequiredService<IOptions<JwtSettings>>();

            option.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettingsOptions.Value.Issuer,
                ValidAudience = jwtSettingsOptions.Value.Audience,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettingsOptions.Value.SecurityKey)),
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddScoped<IUserTokenService, JwtUserTokenManager>();

        return services;
    }

    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IUserService, CoreIdentityUserManager>();

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