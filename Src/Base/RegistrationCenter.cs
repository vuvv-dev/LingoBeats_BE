using System.Security.Cryptography;
using System.Text;
using Base.Common.DependencyInjection;
using Base.Config;
using Microsoft.IdentityModel.Tokens;

namespace Base;

public static class RegistrationCenter
{
    public static IServiceCollection Register(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        AddExternalServices(services, configuration);
        AddOptions(services, configuration);
        return services;
    }

    private static IServiceCollection AddExternalServices(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        var externalServicesRegisterType = typeof(IExternalServiceRegister);
        var currentAssembly = typeof(RegistrationCenter).Assembly;

        var allTypes = currentAssembly.GetTypes();

        var areServicesFound = allTypes.Any(type =>
            externalServicesRegisterType.IsAssignableFrom(type) && !type.IsInterface
        );

        if (!areServicesFound)
        {
            throw new ApplicationException(
                $"No Registration of external modules are found in this assembly {currentAssembly.GetName()}, please check again !!"
            );
        }

        foreach (var type in allTypes)
        {
            if (externalServicesRegisterType.IsAssignableFrom(type) && !type.IsInterface)
            {
                var register = Activator.CreateInstance(type) as IExternalServiceRegister;
                register?.Register(services, configuration);
            }
        }

        return services;
    }

    private static IServiceCollection AddOptions(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        var addAuthOptions =
            configuration
                .GetRequiredSection("Authentication")
                .GetRequiredSection("Jwt")
                .GetRequiredSection("User")
                .Get<JwtAuthenticationOption>() ?? throw new InvalidOperationException();

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = addAuthOptions.ValidateIssuer,
            ValidateAudience = addAuthOptions.ValidateAudience,
            ValidateLifetime = addAuthOptions.ValidateLifetime,
            ValidateIssuerSigningKey = addAuthOptions.ValidateIssuerSigningKey,
            ValidIssuer = addAuthOptions.ValidIssuer,
            ValidAudience = addAuthOptions.ValidAudience,
            ValidTypes = addAuthOptions.ValidTypes,
            IssuerSigningKey = new SymmetricSecurityKey(
                new HMACSHA256(Encoding.UTF8.GetBytes(addAuthOptions.IssuerSigningKey)).Key
            ),
        };

        services.AddSingleton(tokenValidationParameters);

        var aspNetCoreIdentityOption = configuration
            .GetRequiredSection("AspNetCoreIdentity")
            .Get<AspNetCoreIdentityOption>();

        services.AddSingleton(aspNetCoreIdentityOption ?? new AspNetCoreIdentityOption());
        return services;
    }
}
