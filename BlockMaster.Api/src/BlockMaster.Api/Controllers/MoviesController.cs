using Microsoft.AspNetCore.Mvc;

namespace BlockMaster.Api.Controllers;

[Route("[controller]")]
public class MoviesController : ControllerBase
{
    [HttpGet]
    public IEnumerable<string> Get()
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}")]
    public string Get(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public void Post([FromBody] string value)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        throw new NotImplementedException();
    }
}