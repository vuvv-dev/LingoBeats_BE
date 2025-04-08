using Base.Common.DependencyInjection;
using Base.DataBaseAndIdentity.DBContext;
using Base.DataBaseAndIdentity.Entities;
using FCommon.AccessToken;
using FCommon.Authorization;
using FCommon.RefreshToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FCommon;

public class RegistrationCenter : IServiceRegister
{
    public IServiceCollection Register(IServiceCollection services, IConfiguration configuration)
    {
        AddAppDefindedServices(services, configuration);
        AddDefaultAuthorization(services, configuration);
        return services;
    }

    private static void AddAppDefindedServices(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        #region Core
        services
            .AddSingleton<IAppAccessTokenHandler, AppAccessTokenHandler>()
            .MakeSingletonLazy<IAppAccessTokenHandler>(); //AccessToken>

        services
            .AddSingleton<IAppRefreshTokenHandler, AppRefreshTokenHandler>()
            .MakeSingletonLazy<IAppRefreshTokenHandler>();

        services
            .MakeScopedLazy<AppDbContext>()
            .MakeScopedLazy<UserManager<IdentityUserEntity>>()
            .MakeScopedLazy<RoleManager<IdentityRoleEntity>>()
            .MakeScopedLazy<SignInManager<IdentityUserEntity>>();
        #endregion
    }

    internal static void AddDefaultAuthorization(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        #region Authorization
        services
            .AddAuthorizationBuilder()
            .AddDefaultPolicy(
                nameof(DefaultAuthorizationRequirement),
                policy =>
                    policy
                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser()
                        .AddRequirements(new DefaultAuthorizationRequirement())
            );
        services.AddSingleton<IAuthorizationHandler, DefaultAuthorizationRequirementHandler>();
        #endregion
    }
}
