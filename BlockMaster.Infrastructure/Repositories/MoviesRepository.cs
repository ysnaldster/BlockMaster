using System.Net;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using BlockMaster.Domain.Entities;
using BlockMaster.Domain.Util;
using BlockMaster.Infrastructure.Helpers;
using Serilog;
using Exception = System.Exception;

namespace BlockMaster.Infrastructure.Repositories;

public class MoviesRepository
{
    #region private attributes

    private readonly IAmazonDynamoDB _amazonDynamoDb;
    private readonly string _moviesTableName;
    private static Table? _moviesTable;

    #endregion

    #region public methods

    public MoviesRepository(IAmazonDynamoDB amazonDynamoDb, string moviesTableName)
    {
        _amazonDynamoDb = amazonDynamoDb;
        _moviesTableName = moviesTableName;
        _moviesTable = Table.LoadTable(_amazonDynamoDb, _moviesTableName);
    }

    public async Task<Movie> CreateAsync(Movie movie)
    {
        var request = new PutItemRequest()
        {
            TableName = _moviesTableName,
            Item = ParseMovieToDictionary(movie)
        };
        var response = await _amazonDynamoDb!.PutItemAsync(request);

        return response.HttpStatusCode == HttpStatusCode.OK
            ? movie
            : throw new InternalServerErrorException(ExceptionUtil.InternalServerErrorMessage);
    }

    public async Task<List<Movie>> FindAsync(string movieName = null!)
    {
        try
        {
            var response = await MovieHelper.ScanAsync(_moviesTable!, movieName);

            return response;
        }
        catch (Exception e)
        {
            Log.Error($"{e.Message}-{e.StackTrace}");
            throw new InternalServerErrorException(ExceptionUtil.InternalServerErrorMessage);
        }
    }

    public async Task<Movie> UpdateAsync(Movie movie)
    {
        try
        {
            var movieToDictionary = ParseMovieToDocument(movie);
            await _moviesTable!.UpdateItemAsync(movieToDictionary);

            return movie;
        }
        catch (Exception e)
        {
            Log.Error($"{e.Message}-{e.StackTrace}");
            throw new InternalServerErrorException(ExceptionUtil.InternalServerErrorMessage);
        }
    }

    public async Task<Movie> DeleteAsync(Movie movie)
    {
        var scanFilter = new ScanFilter();
        scanFilter.AddCondition("Name", ScanOperator.Equal, movie.Name);
        var scanOperation = new ScanOperationConfig()
        {
            Filter = scanFilter
        };
        var search = _moviesTable!.Scan(scanOperation);
        var itemToDelete = (await search.GetRemainingAsync()).Single();
        var batchWrite = _moviesTable.CreateBatchWrite();
        batchWrite.AddItemToDelete(itemToDelete);
        var result = batchWrite.ExecuteAsync();

        return result.IsCompletedSuccessfully
            ? movie
            : throw new InternalServerErrorException(ExceptionUtil.InternalServerErrorMessage);
    }

    #endregion

    #region private methods

    private static Dictionary<string, AttributeValue> ParseMovieToDictionary(Movie movie)
    {
        var movieToDictionary = new Dictionary<string, AttributeValue>()
        {
            { "Id", new AttributeValue { N = movie.Id.ToString() } },
            { "Name", new AttributeValue { S = movie.Name } },
            { "Description", new AttributeValue { S = movie.Description } },
            { "Score", new AttributeValue { S = movie.Score.ToString() } },
            { "Category", new AttributeValue { S = movie.Category } }
        };

        return movieToDictionary;
    }

    private static Document ParseMovieToDocument(Movie movie)
    {
        var movieToDocument = new Document()
        {
            ["Id"] = movie.Id,
            ["Name"] = movie.Name,
            ["Description"] = movie.Description,
            ["Score"] = movie.Score,
            ["Category"] = movie.Category,
        };

        return movieToDocument;
    }

    #endregion
}