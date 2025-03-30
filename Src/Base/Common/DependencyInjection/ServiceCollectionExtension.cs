using System.Reflection;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Base.Common.DependencyInjection;

public static class ServiceCollectionExtension
{
    private static readonly Type AsyncActionFilterType = typeof(IAsyncActionFilter);

    public static IServiceCollection MakeSingletonLazy<T>(this IServiceCollection services)
        where T : class
    {
        return services.AddSingleton<Lazy<T>>(provider => new(provider.GetRequiredService<T>()));
    }

    public static IServiceCollection MakeScopedLazy<T>(this IServiceCollection services)
        where T : class
    {
        return services.AddScoped<Lazy<T>>(provider => new(provider.GetRequiredService<T>()));
    }

    public static IServiceCollection RegisterFilterFromAssembly(
        this IServiceCollection services,
        Assembly assembly
    )
    {
        var allTypes = assembly.GetTypes();
        var areFiltersFound = allTypes.Any(type =>
            AsyncActionFilterType.IsAssignableFrom(type) && !type.IsInterface
        );
        if (!areFiltersFound)
        {
            throw new ApplicationException(
                $"No filters found in assembly {assembly.FullName}, please omit this function!!!"
            );
        }
        foreach (var type in allTypes)
        {
            if (AsyncActionFilterType.IsAssignableFrom(type) && !type.IsInterface)
            {
                services.AddSingleton(type);
            }
        }
        return services;
    }
}
