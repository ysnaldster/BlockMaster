using BlockMaster.Api.Middleware;
using BlockMaster.Domain.Request;
using BlockMaster.Domain.Services;
using BlockMaster.Domain.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BlockMaster.Api.Controllers;

[EnableCors(ConstUtil.AllowCorsPolicy)]
[Authorize]
[Route("[controller]")]
[ApiVersion("1")]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] MovieRequest movieRequest)
    {
        var response = await _movieService.Create(movieRequest);

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _movieService.FindAll();

        return Ok(response);
    }

    [HttpGet("{movieName}")]
    public async Task<IActionResult> Get(string movieName)
    {
        var response = await _movieService.Find(movieName);

        return Ok(response);
    }

    [HttpPut("{movieName}")]
    [IdentityMiddleware(IdentityUtil.AdminUserClaimName!, IdentityUtil.AdminUserClaimValue!)]
    public async Task<IActionResult> Put(string movieName, [FromBody] MovieRequest movieRequest)
    {
        var response = await _movieService.Update(movieName, movieRequest);

        return Ok(response);
    }

    [HttpDelete("{movieName}")]
    [IdentityMiddleware(IdentityUtil.AdminUserClaimName!, IdentityUtil.AdminUserClaimValue!)]
    public async Task<IActionResult> Delete(string movieName)
    {
        var response = await _movieService.Delete(movieName);

        return Ok(response);
    }
}