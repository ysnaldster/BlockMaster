using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using BlockMaster.Tests.Configuration;
using Testcontainers.LocalStack;
using Xunit;

namespace BlockMaster.Tests.Containers;

public class DynamoDbContainer : IAsyncLifetime
{
    #region private attributes

    private readonly LocalStackContainer _dynamoDbBuilder = new LocalStackBuilder()
        .WithPortBinding(4566, 4566)
        .WithCleanUp(true)
        .Build();

    #endregion

    #region public methods

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
    
    public async Task PopulateDynamoDb()
    {
        await LocalDynamoDbConfiguration.PopulateDynamoDb();
    }

    #endregion

    #region private methods

    private static async Task ConfigureLocalStackContainer()
    {
        await LocalSystemManagerConfiguration.ConfigureParameterStore();
        await LocalDynamoDbConfiguration.ConfigureDynamoDb();
    }

    #endregion
}