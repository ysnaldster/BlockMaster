using BlockMaster.Domain.Entities;
using BlockMaster.Domain.Request;

namespace BlockMaster.Domain.Services;

public interface IMovieService
{
    Task<Movie> Create(MovieRequest movieRequest);
    Task<List<Movie>> FindAll();
    Task<Movie> Find(string movieName);
    Task<Movie> Update(string movieName, MovieRequest movieRequest);
    Task<Movie> Delete(string movieName);
}