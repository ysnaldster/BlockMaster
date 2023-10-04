using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using BlockMaster.Domain.Entities;
using BlockMaster.Tests.Util;
using BlockMaster.Tests.Util.Entity;
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
        await InsertMovie("../../../Util/JsonFiles/GetMovies.json");
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
        var moviesList = JsonConvert.DeserializeObject<List<Movie>>(json);
        foreach (var movieSerialize in moviesList.Select(movie => new
                 {
                     movie.Id,
                     movie.Name,
                     movie.Description,
                     movie.Country,
                     Score = movie.Score.ToString(),
                     movie.Category
                 }).Select(JsonConvert.SerializeObject))
        {
            await table.PutItemAsync(Document.FromJson(movieSerialize));
        }
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