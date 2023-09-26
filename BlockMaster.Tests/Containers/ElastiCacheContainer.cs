using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Xunit;

namespace BlockMaster.Tests.Containers;

public class ElastiCacheContainer : IAsyncLifetime
{
    #region private attributes

    private readonly IContainer _redisContainer = new ContainerBuilder()
        .WithImage("redis")
        .WithCleanUp(true)
        .WithPortBinding(6379, 6379)
        .Build();

    #endregion

    #region public methods

    public async Task InitializeAsync()
    {
        await _redisContainer!.StartAsync()
            .ConfigureAwait(false);
    }

    public async Task DisposeAsync()
    {
        await _redisContainer!.DisposeAsync();
    }

    #endregion
}