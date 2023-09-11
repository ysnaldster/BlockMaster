using BlockMaster.Business.Services;
using BlockMaster.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BlockMaster.Api.Controllers;

[Route("[controller]")]
public class MoviesController : ControllerBase
{
    private readonly MovieService _movieService;

    public MoviesController(MovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Movie movie)
    {
        var response = await _movieService.Create(movie);
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
        var response = await _movieService.FindByName(movieName);
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] Movie movie)
    {
        var response = await _movieService.Update(movie);
        return Ok(response);
    }

    [HttpPut("{movieName}")]
    public async Task<IActionResult> Delete(string movieName)
    {
        var response = await _movieService.Delete(movieName);
        return Ok(response);
    }
}