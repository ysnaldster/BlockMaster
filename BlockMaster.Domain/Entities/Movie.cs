using BlockMaster.Domain.Request;

namespace BlockMaster.Domain.Entities;

public class Movie
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public double? Score { get; set; }
    public string? Category { get; set; }

    public Movie()
    {
    }

    public Movie(long id, MovieRequest movieRequest)
    {
        Id = id;
        Name = movieRequest.Name;
        Description = movieRequest.Description;
        Score = movieRequest.Score;
        Category = movieRequest.Category;
    }
}