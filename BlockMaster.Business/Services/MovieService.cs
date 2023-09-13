using BlockMaster.Domain.Entities;
using BlockMaster.Domain.Exceptions.NotFoundException;
using BlockMaster.Domain.Request;
using BlockMaster.Domain.Util;
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

    public async Task<Movie> Create(MovieRequest movieRequest)
    {
        var movieId = await GenerateSequenceId();
        var request = new Movie(movieId, movieRequest);
        var response = await _movieRepository.CreateAsync(request);

        return response;
    }

    public async Task<List<Movie>> FindAll()
    {
        var response = await _movieRepository.FindAsync();
        if (!response.Any())
        {
            throw new MovieNotFoundException(ExceptionUtil.MoviesNotFoundExceptionMessage);
        }

        return response;
    }

    public async Task<List<Movie>> FindByName(string movieName)
    {
        var response = await _movieRepository.FindAsync(movieName);
        if (!response.Any())
        {
            throw new MovieNotFoundException(ExceptionUtil.MovieNotFoundExceptionMessage);
        }

        return response;
    }

    public async Task<Movie> Update(string movieName, MovieRequest movieRequest)
    {
        var movies = await _movieRepository.FindAsync(movieName);
        if (!movies.Any())
        {
            throw new MovieNotFoundException(ExceptionUtil.MovieNotFoundExceptionMessage);
        }

        var request = new Movie(movies.Single().Id, movieRequest);
        var response = await _movieRepository.UpdateAsync(request);

        return response;
    }

    public async Task<Movie> Delete(string movieName)
    {
        var movieToDelete = new Movie()
        {
            Name = movieName
        };
        var response = await _movieRepository.DeleteAsync(movieToDelete);

        return response;
    }

    #endregion

    #region private methods

    private async Task<long> GenerateSequenceId()
    {
        var movies = await _movieRepository.FindAsync();
        var actualMovieId = movies.MaxBy(movieItem => movieItem.Id)!.Id++;

        return actualMovieId + 1;
    }

    #endregion
}