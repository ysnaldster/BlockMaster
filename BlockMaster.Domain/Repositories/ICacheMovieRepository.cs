using BlockMaster.Domain.Entities;

namespace BlockMaster.Domain.Repositories;

public interface ICacheMovieRepository
{
    Task CreateHash(Movie movie);
    Task<Movie?> FindHash(string movieName);
}