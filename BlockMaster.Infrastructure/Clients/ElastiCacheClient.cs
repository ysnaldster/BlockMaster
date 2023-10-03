using StackExchange.Redis;

namespace BlockMaster.Infrastructure.Clients;

public class ElastiCacheClient
{
    private const int DefaultDatabase = 0;
    private readonly IDatabase _database;

    public ElastiCacheClient(string endpoint)
    {
        var redisConnection = ConnectionMultiplexer.Connect(endpoint);
        _database = redisConnection.GetDatabase(DefaultDatabase);
    }

    public IDatabase GetDatabase()
    {
        return _database;
    }
}