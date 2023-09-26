using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
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
        var config = new AmazonDynamoDBConfig
        {
            RegionEndpoint = RegionEndpoint.USEast1,
            ServiceURL = "http://localhost:4566"
        };
    }

    public async Task DisposeAsync()
    {
        await _dynamoDbBuilder.StopAsync();
    }

    #endregion
}