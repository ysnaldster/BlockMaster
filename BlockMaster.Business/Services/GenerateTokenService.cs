using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlockMaster.Domain.Request.Identity;
using BlockMaster.Domain.Services;
using Microsoft.IdentityModel.Tokens;

namespace BlockMaster.Business.Services;

public class GenerateTokenService : IGenerateTokenService
{
    private readonly string _apiKey;

    private const string
        Issuer = "http://localhost:5000/",
        Audience = "http://localhost:5000/";

    public GenerateTokenService(string apiKey)
    {
        _apiKey = apiKey;
    }

    public string GenerateToken(TokenGenerationRequest request)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_apiKey);
        var tokenLifeTime = TimeSpan.FromHours(8);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, request.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Email, request.Email ?? string.Empty),
            new("userid", request.UserId ?? string.Empty)
        };

        claims.AddRange(from claimPair in request.CustomClaims ?? new Dictionary<string, object>()
            let valueType = claimPair.Value switch
            {
                bool _ => ClaimValueTypes.Boolean,
                double _ => ClaimValueTypes.Double,
                _ => ClaimValueTypes.String
            }
            select new Claim(claimPair.Key, claimPair.Value.ToString() ?? string.Empty, valueType));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(tokenLifeTime),
            Issuer = Issuer,
            Audience = Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return jwt;
    }
}