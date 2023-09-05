using Microsoft.AspNetCore.Mvc;

namespace BlockMaster.Api.Controllers;

[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("HelloWord");
    }
}