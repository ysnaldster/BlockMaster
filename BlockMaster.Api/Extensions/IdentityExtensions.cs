using System.Text;
using BlockMaster.Domain.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BlockMaster.Api.Extensions;

public static class IdentityExtensions
{
    public static void SetAuthenticationStructure(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters()
            {
            //Choose to SecretManager
                ValidIssuer = "http://localhost:5000/",
                ValidAudience = "http://localhost:5000/",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("clave_secreta_de_al_menos_32_bytes")),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });
    }

    public static void SetAuthorizationStructure(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAuthorization(option => option.AddPolicy(IdentityUtil.AdminUserClaimValue!, p =>
            p.RequireClaim(IdentityUtil.AdminUserClaimName!, "true")));
    }
}