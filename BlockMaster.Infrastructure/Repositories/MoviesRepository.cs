using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using BlockMaster.Domain.Entities;

namespace BlockMaster.Infrastructure.Repositories;

public class MoviesRepository
{
    private readonly IAmazonDynamoDB _amazonDynamoDb;
    private readonly string _moviesTableName;

    public MoviesRepository(IAmazonDynamoDB amazonDynamoDb, string moviesTableName)
    {
        _amazonDynamoDb = amazonDynamoDb;
        _moviesTableName = moviesTableName;
    }

    public async Task<List<Movie>> FindAllAsync(string movieName)
    {
        var scanFilter = new ScanFilter();
        scanFilter.AddCondition("Name", ScanOperator.Equal, movieName);
        var scanOperation = new ScanOperationConfig()
        {
            Filter = scanFilter
        };
        var tablaConfig = Table.LoadTable(_amazonDynamoDb, _moviesTableName);
        var search = tablaConfig!.Scan(scanOperation);
        var moviesList = new List<Movie>();
        do
        {
            var results = await search.GetNextSetAsync();
            if (results.Count > 0)
            {
                
            }
        } while (!search.IsDone);

        return moviesList.Count == 0 ? throw new Exception() : moviesList;
    }
}