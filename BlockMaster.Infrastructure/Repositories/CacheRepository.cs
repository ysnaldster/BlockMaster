using System.Text.Json;
using BlockMaster.Domain.Entities;
using BlockMaster.Infrastructure.Clients;

namespace BlockMaster.Infrastructure.Repositories;

public class CacheRepository
{
    #region private attibutes

    private readonly ElastiCacheClient _elastiCacheClient;
    private const string HashMainKey = "Movies";

    #endregion

    #region public methods

    public CacheRepository(ElastiCacheClient elastiCacheClient)
    {
        _elastiCacheClient = elastiCacheClient;
    }

    public async Task CreateMovieHash(Movie movie)
    {
        var redisConnection = _elastiCacheClient.GetDatabase();
        var movieValue = JsonSerializer.Serialize(movie);
        await redisConnection.HashSetAsync(HashMainKey, movie.Name, movieValue);
    }

    public async Task<Movie?> FindMovieHash(string movieName)
    {
        var redisConnection = _elastiCacheClient.GetDatabase();
        var getMovieHash = await redisConnection.HashGetAsync(HashMainKey, movieName);
        return getMovieHash.HasValue
            ? JsonSerializer.Deserialize<Movie>(getMovieHash!)
            : null;
    }

    #endregion
}