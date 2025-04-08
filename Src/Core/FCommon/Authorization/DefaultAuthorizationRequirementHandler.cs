using System.Security.Claims;
using FCommon.AccessToken;
using FCommon.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace FCommon.Authorization;

public sealed class DefaultAuthorizationRequirementHandler
    : AuthorizationHandler<DefaultAuthorizationRequirement>
{
    private readonly Lazy<IHttpContextAccessor> _httpContextAccesor;

    public DefaultAuthorizationRequirementHandler(Lazy<IHttpContextAccessor> httpContextAccesor)
    {
        _httpContextAccesor = httpContextAccesor;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        DefaultAuthorizationRequirement requirement
    )
    {
        if (!context.User.Identity.IsAuthenticated)
        {
            context.Fail();
            return Task.CompletedTask;
        }
        var expClaimValue =
            context.User.FindFirstValue(AppContant.JsonWebToken.ClaimType.EXP) ?? string.Empty;
        var tokenExpireTime = AppAccessTokenHandler.IsAccessTokenExpired(expClaimValue);
        if (tokenExpireTime)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var purposeClaimValue = context.User.FindFirstValue(
            AppContant.JsonWebToken.ClaimType.PURPOSE.Name
        );

        if (
            !Equals(
                objA: purposeClaimValue,
                objB: AppContant.JsonWebToken.ClaimType.PURPOSE.Value.USER_IN_APP
            )
        )
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var httpContext = _httpContextAccesor.Value.HttpContext;

        if (httpContext != null)
        {
            httpContext.Items.Add(
                AppContant.JsonWebToken.ClaimType.SUB,
                context.User.FindFirstValue(AppContant.JsonWebToken.ClaimType.SUB)
            );
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
