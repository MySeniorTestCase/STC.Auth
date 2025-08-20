using Scalar.AspNetCore;
using STC.Auth.Application;
using STC.Auth.Infrastructure;
using STC.Auth.Persistence.EfCore;
using STC.Auth.WebAPI.ApiGroups;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

builder.Services.AddAuthorization();

builder.Services.AddApplicationDependencies()
    .AddInfrastructureDependencies(configuration: builder.Configuration)
    .AddEfCorePersistenceDependencies(configuration: builder.Configuration);

WebApplication app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseAuthentication().UseAuthorization();

app.MapUsersApi();

app.Run();