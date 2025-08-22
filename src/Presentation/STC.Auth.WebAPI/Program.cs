using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using STC.Auth.Application;
using STC.Auth.Application.Features.Seeds.Services;
using STC.Auth.Infrastructure;
using STC.Auth.Persistence.EfCore;
using STC.Auth.WebAPI.ApiGroups;
using STC.Auth.WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddAuthorization();

builder.Services.AddScoped<GlobalExceptionHandlerMiddleware>();

builder.Services.AddApplicationDependencies()
    .AddInfrastructureDependencies(configuration: builder.Configuration, loggingBuilder: builder.Logging)
    .AddEfCorePersistenceDependencies(configuration: builder.Configuration);

WebApplication app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.MapOpenApi();
app.MapScalarApiReference(_ => _.Servers = []);
app.UseAuthentication().UseAuthorization();
app.MapUsersApi().MapRolesApi();

await app.Services.CreateAsyncScope().ServiceProvider.GetRequiredService<AuthDbContext>().Database.MigrateAsync();

await app.Services.CreateAsyncScope().ServiceProvider.GetRequiredService<ISeedDataService>()
    .CheckAndAddSeedAsync(cancellationToken: CancellationToken.None);

app.Run();