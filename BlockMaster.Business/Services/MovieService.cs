using BlockMaster.Domain.Entities;
using BlockMaster.Infrastructure.Repositories;

namespace BlockMaster.Business.Services;

public class MovieService
{
    #region private attributes

    private readonly MoviesRepository _movieRepository;

    #endregion

    #region public methods

    public MovieService(MoviesRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<Movie> Create(Movie movie)
    {
        var response = await _movieRepository.CreateAsync(movie);
        return response;
    }

    public async Task<List<Movie>> FindAll()
    {
        var response = await _movieRepository.FindAsync();
        return response;
    }
    public async Task<List<Movie>> FindByName(string movieName)
    {
        var response = await _movieRepository.FindAsync(movieName);
        return response;
    }

    #endregion
}