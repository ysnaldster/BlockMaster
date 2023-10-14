using System.Threading.Tasks;
using BlockMaster.Tests.Configuration;
using Testcontainers.LocalStack;
using Xunit;

namespace BlockMaster.Tests.Containers;

public class LocalStackContainer : IAsyncLifetime
{
    private const int LocalStackPort = 4566;

    private readonly Testcontainers.LocalStack.LocalStackContainer _dynamoDbBuilder = new LocalStackBuilder()
        .WithPortBinding(LocalStackPort, LocalStackPort)
        .WithCleanUp(true)
        .Build();

    public async Task InitializeAsync()
    {
        await _dynamoDbBuilder.StartAsync()
            .ConfigureAwait(false);
        await ConfigureLocalStackContainer();
    }

    public async Task DisposeAsync()
    {
        await _dynamoDbBuilder.StopAsync();
    }

    public static async Task PopulateDynamoDb()
    {
        await LocalDynamoDbConfiguration.PopulateDynamoDb();
    }

    public static async Task ClearDynamoDb()
    {
        await LocalDynamoDbConfiguration.ClearDynamoDb();
    }

    private static async Task ConfigureLocalStackContainer()
    {
        await LocalSystemManagerConfiguration.ConfigureParameterStore();
        await LocalDynamoDbConfiguration.ConfigureDynamoDb();
    }
}