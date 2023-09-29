using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using BlockMaster.Tests.Util;
using DynamoConverter;

namespace BlockMaster.Tests.Configuration;

public class LocalDynamoDbConfiguration
{
    #region attributes

    private static readonly AmazonDynamoDBConfig DynamoConfig = new()
    {
        RegionEndpoint = RegionEndpoint.GetBySystemName(ConstUtil.AwsRegion),
        ServiceURL = ConstUtil.AwsHost
    };

    private static readonly AmazonDynamoDBClient DynamoDbClient =
        new(ConstUtil.AwsAccessKey, ConstUtil.AwsSecretAccessKey, DynamoConfig);

    #endregion

    #region public methods

    public static async Task ConfigureDynamoDb()
    {
        await CreateTable();
    }

    public static async Task PopulateDynamoDb()
    {
        foreach (var request in MoviesUtil.CreateMovieRecords()
                     .Select(movieItem => new
                     {
                         movieItem.Id,
                         movieItem.Name,
                         movieItem.Description,
                         movieItem.Country,
                         movieItem.Score,
                         movieItem.Category
                     }).Select(movieBody => new PutItemRequest()
                     {
                         TableName = ConstUtil.MovieTableName,
                         Item = DynamoConvert.SerializeObject(movieBody)
                     }))
        {
            await DynamoDbClient!.PutItemAsync(request);
        }
    }

    #endregion

    #region private methods

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

    private static async Task<bool> TableAlreadyExists(string tableName)
    {
        var request = new ListTablesRequest
        {
            Limit = 10
        };

        var tables = await DynamoDbClient.ListTablesAsync(request);

        return tables.TableNames.Contains(tableName);
    }

    #endregion
}