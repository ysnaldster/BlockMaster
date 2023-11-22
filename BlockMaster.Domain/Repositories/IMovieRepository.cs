using BlockMaster.Domain.Entities;

namespace BlockMaster.Domain.Repositories;

public interface IMovieRepository
{
    Task<Movie> CreateAsync(Movie movie);
    Task<List<Movie>> FindAsync(string movieName = null!);
    Task<Movie> UpdateAsync(Movie movie);
    Task<Movie> DeleteAsync(Movie movie);
}