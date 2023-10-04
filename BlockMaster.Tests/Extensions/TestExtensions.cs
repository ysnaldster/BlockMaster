using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BlockMaster.Domain.Entities;
using Newtonsoft.Json;

namespace BlockMaster.Tests.Extensions;

public class TestExtensions
{
    protected static async Task<List<Movie>> GetMoviesFromStreamReader(string path)
    {
        using var streamReader = new StreamReader(path);
        var stringResult = await streamReader.ReadToEndAsync();
        var movie = JsonConvert.DeserializeObject<List<Movie>>(stringResult);

        return movie;
    }
}