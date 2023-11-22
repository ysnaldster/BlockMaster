using System.Text.Json;
using BlockMaster.Domain.Entities;
using BlockMaster.Domain.Repositories;
using BlockMaster.Infrastructure.Clients;

namespace BlockMaster.Infrastructure.Repositories;

public class CacheMovieRepository : ICacheMovieRepository
{
    private readonly ElastiCacheClient _elastiCacheClient;
    private const string HashMainKey = "Movies";
    
    public CacheMovieRepository(ElastiCacheClient elastiCacheClient)
    {
        _elastiCacheClient = elastiCacheClient;
    }

    public async Task CreateHash(Movie movie)
    {
        var redisConnection = _elastiCacheClient.GetDatabase();
        var movieValue = JsonSerializer.Serialize(movie);

        await redisConnection.HashSetAsync(HashMainKey, movie.Name, movieValue);
    }

    public async Task<Movie?> FindHash(string movieName)
    {
        var redisConnection = _elastiCacheClient.GetDatabase();
        var getMovieHash = await redisConnection.HashGetAsync(HashMainKey, movieName);
        return getMovieHash.HasValue
            ? JsonSerializer.Deserialize<Movie>(getMovieHash!)
            : null;
    }
}