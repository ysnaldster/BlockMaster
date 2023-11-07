using BlockMaster.Business.Services;
using BlockMaster.Domain.Request.Identity;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BlockMaster.Api.Controllers;

[Route("[controller]")]
[EnableCors("AllowCorsPolicy")]
[ApiVersion("1")]
public class IdentityController : ControllerBase
{
    private readonly GenerateTokenService _generateTokenService;

    public IdentityController(GenerateTokenService generateTokenService)
    {
        _generateTokenService = generateTokenService;
    }


    [HttpPost("token")]
    public IActionResult GenerateToken([FromBody] TokenGenerationRequest request)
    {
        var jwt = _generateTokenService.GenerateToken(request);

        return Ok(new { token = jwt });
    }
}