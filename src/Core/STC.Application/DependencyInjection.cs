using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using STC.Application.BehaviorPipelines.Validation;

namespace STC.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.AddMediatR(_x => _x.RegisterServicesFromAssemblies(assemblies));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviorPipeline<,>));
        services.AddValidatorsFromAssemblies(assemblies);

        return services;
    }
}