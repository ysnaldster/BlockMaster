using System.IO;
using System.Threading.Tasks;
using BlockMaster.Domain.Entities;
using Newtonsoft.Json;

namespace BlockMaster.Tests.Extensions;

public class TestExtensions
{
    protected static async Task<Movie> GetMovieFromStreamReader(string path)
    {
        using var streamReader = new StreamReader(path);
        var stringResult = await streamReader.ReadToEndAsync();
        var movie = JsonConvert.DeserializeObject<Movie>(stringResult);

        return movie;
    }
}