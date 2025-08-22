using Scalar.AspNetCore;
using STC.Auth.Application;
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
app.MapScalarApiReference();
app.UseAuthentication().UseAuthorization();
app.MapUsersApi().MapRolesApi();

app.Run();