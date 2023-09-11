using System.Net;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using BlockMaster.Domain.Entities;
using BlockMaster.Infrastructure.Helpers;

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
            Item = CreateMovieStructure(movie)
        };
        var response = await _amazonDynamoDb!.PutItemAsync(request);

        return response.HttpStatusCode == HttpStatusCode.OK ? movie : throw new Exception();
    }

    public async Task<List<Movie>> FindAsync(string movieName = null!)
    {
        var response = await MovieHelper.ScanAsync(_moviesTable!, movieName);
        return response;
    }

    #endregion

    #region private methods

    private static Dictionary<string, AttributeValue> CreateMovieStructure(Movie movie)
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

    #endregion
}