using BlockMaster.Domain.Entities;
using BlockMaster.Domain.Request;
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
        var movies = await _movieRepository.FindAsync();
        var movieId = movies.MaxBy(movieItem => movieItem.Id)!.Id;
        var request = new Movie(movieId, movieRequest);
        var response = await _movieRepository.CreateAsync(request);
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

    public async Task<Movie> Update(MovieRequest movieRequest)
    {
        //Validate When Exist > 0 (Movies) && Exception Not Found || Conflict > 1
        var movie = (await _movieRepository.FindAsync(movieRequest.Name!)).Single();
        var request = new Movie(movie.Id, movieRequest);
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
}