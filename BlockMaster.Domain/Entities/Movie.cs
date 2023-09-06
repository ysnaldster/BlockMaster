namespace BlockMaster.Domain.Entities;

public class Movie
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public double? Score { get; set; }
    public string? Category { get; set; }
}