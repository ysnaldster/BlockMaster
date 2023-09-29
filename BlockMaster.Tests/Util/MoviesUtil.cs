using System.Collections.Generic;
using BlockMaster.Domain.Entities;

namespace BlockMaster.Tests.Util;

public static class MoviesUtil
{
    #region public methods

    public static IEnumerable<Movie> CreateMovieRecords()
    {
        var healthCheckList = new List<Movie>()
        {
            new()
            {
                Id = 1,
                Name = "Test",
                Description = "Test",
                Country = "CO",
                Score = 4,
                Category = "Test"
            }
        };

        return healthCheckList;
    }

    #endregion
}