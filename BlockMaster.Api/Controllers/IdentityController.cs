using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlockMaster.Domain.Request.Identity;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BlockMaster.Api.Controllers;

[Route("[controller]")]
[EnableCors("AllowCorsPolicy")]
[ApiVersion("1")]
public class IdentityController : ControllerBase
{
    private const string TokenSecret = "clave_secreta_de_al_menos_32_bytes";
    private static readonly TimeSpan TokenLifeTime = TimeSpan.FromHours(8);


    [HttpPost("token")]
    public IActionResult GenerateToken([FromBody] TokenGenerationRequest request)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(TokenSecret);

        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Sub, request.Email ?? string.Empty),
            new (JwtRegisteredClaimNames.Email, request.Email ?? string.Empty),
            new ("userid", request.UserId ?? string.Empty)
        };

        foreach (var claimPair in request.CustomClaims ?? new Dictionary<string, object>())
        {
            var valueType = claimPair.Value switch
            {
                bool _ => ClaimValueTypes.Boolean,
                double _ => ClaimValueTypes.Double,
                _ => ClaimValueTypes.String
            };

            var claim = new Claim(claimPair.Key, claimPair.Value.ToString() ?? string.Empty, valueType);
            claims.Add(claim);
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(TokenLifeTime),
            Issuer = "http://localhost:5000/",
            Audience = "http://localhost:5000/",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return Ok(new { token = jwt });
    }
}