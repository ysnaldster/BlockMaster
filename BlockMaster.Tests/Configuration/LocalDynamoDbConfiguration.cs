using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using BlockMaster.Domain.Entities;
using BlockMaster.Tests.Util;
using Newtonsoft.Json;

namespace BlockMaster.Tests.Configuration;

public static class LocalDynamoDbConfiguration
{
    private static readonly AmazonDynamoDBConfig DynamoConfig = new()
    {
        RegionEndpoint = RegionEndpoint.GetBySystemName(ConstUtil.AwsRegion),
        ServiceURL = ConstUtil.AwsHost
    };

    private static readonly AmazonDynamoDBClient DynamoDbClient =
        new(ConstUtil.AwsAccessKey, ConstUtil.AwsSecretAccessKey, DynamoConfig);

    public static async Task ConfigureDynamoDb()
    {
        await CreateTable();
    }

    public static async Task PopulateDynamoDb()
    {
        await InsertMovie("../../../Util/JsonFiles/GetMovie.json");
    }

    private static async Task CreateTable()
    {
        var request = GetCreateTableRequest();
        if (!await TableAlreadyExists(ConstUtil.MovieTableName))
        {
            await DynamoDbClient.CreateTableAsync(request);
        }
    }

    private static CreateTableRequest GetCreateTableRequest()
    {
        var attributeDefinitions = new List<AttributeDefinition>
        {
            new() { AttributeName = "Id", AttributeType = "N" },
        };

        var keySchemaElements = new List<KeySchemaElement>
        {
            new() { AttributeName = "Id", KeyType = "HASH" }
        };

        return new CreateTableRequest
        {
            TableName = ConstUtil.MovieTableName,
            AttributeDefinitions = attributeDefinitions,
            KeySchema = keySchemaElements,
            ProvisionedThroughput = new ProvisionedThroughput
            {
                ReadCapacityUnits = 5,
                WriteCapacityUnits = 5
            }
        };
    }

    private static async Task InsertMovie(string path)
    {
        var table = Table.LoadTable(DynamoDbClient, ConstUtil.MovieTableName);
        using var stream = new StreamReader(path);
        var json = await stream.ReadToEndAsync();
        var movieObject = JsonConvert.DeserializeObject<Movie>(json);

        var movieObjectToStructure = new
        {
            movieObject.Id,
            movieObject.Name,
            movieObject.Description,
            movieObject.Country,
            Score = movieObject.Score.ToString(),
            movieObject.Category
        };
        var movieSerialize = JsonConvert.SerializeObject(movieObjectToStructure);

        await table.PutItemAsync(Document.FromJson(movieSerialize));
    }

    private static async Task<bool> TableAlreadyExists(string tableName)
    {
        var request = new ListTablesRequest
        {
            Limit = 10
        };

        var tables = await DynamoDbClient.ListTablesAsync(request);

        return tables.TableNames.Contains(tableName);
    }
}