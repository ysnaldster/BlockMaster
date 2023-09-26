using Amazon.DynamoDBv2;

namespace BlockMaster.Tests.Clients;

public class TestDynamoDbClient
{
    #region private attributes

    private readonly AmazonDynamoDBConfig _amazonDynamoDbConfig;

    #endregion

    #region public methods

    public TestDynamoDbClient(AmazonDynamoDBConfig config)
    {
        _amazonDynamoDbConfig = config;
    }

    public IAmazonDynamoDB GetConnection()
    {
        return new AmazonDynamoDBClient("123", "123", _amazonDynamoDbConfig);
    }

    #endregion
}