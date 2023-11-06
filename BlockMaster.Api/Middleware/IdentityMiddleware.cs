using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlockMaster.Api.Middleware;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
public class IdentityMiddleware : Attribute, IAuthorizationFilter
{
    private readonly string _claimName;
    private readonly string _claimValue;

    public IdentityMiddleware(string claimName, string claimValue)
    {
        _claimName = claimName;
        _claimValue = claimValue;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.User.HasClaim(_claimName, _claimValue))
        {
            context.Result = new ForbidResult();
        }
    }
}