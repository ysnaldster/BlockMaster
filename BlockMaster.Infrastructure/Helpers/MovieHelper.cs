﻿using System.Globalization;
using Amazon.DynamoDBv2.DocumentModel;
using BlockMaster.Domain.Entities;

namespace BlockMaster.Infrastructure.Helpers;

public static class MovieHelper
{
    public static async Task<List<Movie>> ScanAsync(Table table, string movieName = null!)
    {
        var moviesList = new List<Movie>();
        var scanFilter = new ScanFilter();
        if (!string.IsNullOrEmpty(movieName))
        {
            scanFilter.AddCondition("Name", ScanOperator.Equal, movieName);
        }

        var scanOperation = new ScanOperationConfig()
        {
            Filter = scanFilter
        };
        var search = table.Scan(scanOperation);
        do
        {
            var results = await search.GetNextSetAsync();
            if (results.Any())
            {
                moviesList.AddRange(results.Select(movie => movie.ToAttributeMap())
                    .Select(movieToDictionary => new Movie
                    {
                        Id = long.Parse(movieToDictionary["Id"].N),
                        Name = movieToDictionary["Name"].S,
                        Country = movieToDictionary.TryGetValue("Country", out var country)
                            ? country.S
                            : null,
                        Description = movieToDictionary["Description"].S,
                        Score = double.TryParse(movieToDictionary["Score"].S, NumberStyles.Float,
                            CultureInfo.InvariantCulture, out var parsedScore)
                            ? Math.Round(parsedScore, 1)
                            : null,
                        Category = movieToDictionary.TryGetValue("Category", out var category)
                            ? category.S
                            : null
                    }));
            }
        } while (!search.IsDone);

        return moviesList;
    }
}