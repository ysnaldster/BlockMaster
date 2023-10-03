using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Xunit;

namespace BlockMaster.Tests.Containers;

public class ElastiCacheContainer : IAsyncLifetime
{
    private const string RedisImageName = "redis";
    private const int RedisPort = 6379;

    private readonly IContainer _redisContainer = new ContainerBuilder()
        .WithImage(RedisImageName)
        .WithCleanUp(true)
        .WithPortBinding(RedisPort, RedisPort)
        .Build();

    public async Task InitializeAsync()
    {
        await _redisContainer!.StartAsync()
            .ConfigureAwait(false);
    }

    public async Task DisposeAsync()
    {
        await _redisContainer!.DisposeAsync();
    }
}