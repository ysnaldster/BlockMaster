using BlockMaster.Business.Util;
using BlockMaster.Domain.Entities;
using BlockMaster.Domain.Exceptions.BadRequestException;
using BlockMaster.Domain.Exceptions.ConflictException;
using BlockMaster.Domain.Exceptions.NotFoundException;
using BlockMaster.Domain.Request;
using BlockMaster.Domain.Util;
using BlockMaster.Infrastructure.Repositories;

namespace BlockMaster.Business.Services;

public class MovieService
{
    private readonly MoviesRepository _movieRepository;
    private readonly CacheRepository _cacheRepository;

    public MovieService(MoviesRepository movieRepository, CacheRepository cacheRepository)
    {
        _movieRepository = movieRepository;
        _cacheRepository = cacheRepository;
    }

    public async Task<Movie> Create(MovieRequest movieRequest)
    {
        ValidateMovieRequest(movieRequest);
        var movieId = await GenerateSequenceId();
        var request = new Movie(movieId, movieRequest);
        var countryName = CountryEvaluator.ConvertCountryCodeToCountryName(movieRequest.CountryCode!);
        request.Country = countryName;
        var isRequestInCache = await _cacheRepository.FindMovieHash(request.Name!);
        if (isRequestInCache == null)
        {
            await _cacheRepository.CreateMovieHash(request);
        }

        await ValidateIfMovieExist(movieRequest.Name!);
        var response = await _movieRepository.CreateAsync(request);

        return response;
    }

    public async Task<List<Movie>> FindAll()
    {
        var response = await _movieRepository.FindAsync();
        if (!response.Any())
        {
            throw new MovieNotFoundException(ConstUtil.MoviesNotFoundExceptionMessage);
        }

        return response;
    }

    public async Task<List<Movie>> FindByName(string movieName)
    {
        var moviesMatches = new List<Movie>();
        var movie = await _cacheRepository.FindMovieHash(movieName);
        if (movie == null)
        {
            moviesMatches = await _movieRepository.FindAsync(movieName);
            if (!moviesMatches.Any())
            {
                throw new MovieNotFoundException(ConstUtil.MovieNotFoundExceptionMessage);
            }

            await _cacheRepository.CreateMovieHash(moviesMatches.Single());
        }
        else
        {
            moviesMatches.Add(movie);
        }

        return moviesMatches;
    }

    public async Task<Movie> Update(string movieName, MovieRequest movieRequest)
    {
        var movies = await _movieRepository.FindAsync(movieName);
        ValidateMovieRequest(movieRequest);
        if (!movies.Any())
        {
            throw new MovieNotFoundException(ConstUtil.MovieNotFoundExceptionMessage);
        }

        var request = new Movie(movies.Single().Id, movieRequest);
        var countryName = CountryEvaluator.ConvertCountryCodeToCountryName(movieRequest.CountryCode!);
        request.Country = countryName;
        var response = await _movieRepository.UpdateAsync(request);

        return response;
    }

    public async Task<Movie> Delete(string movieName)
    {
        var movie = (await _movieRepository.FindAsync(movieName))
            .Find(movie => movie.Name == movieName);
        if (movie == null)
        {
            throw new MovieNotFoundException(ConstUtil.MovieNotFoundExceptionMessage);
        }
        
        var response = await _movieRepository.DeleteAsync(movie);

        return response;
    }

    private static void ValidateMovieRequest(MovieRequest movieRequest)
    {
        var movieRequestValidator = new MovieRequestValidator();
        var validate = movieRequestValidator.Validate(movieRequest).IsValid;
        if (!validate)
        {
            throw new MovieRequestBadRequestException(ConstUtil.MovieRequestBadRequestMessage);
        }
    }

    private async Task<long> GenerateSequenceId()
    {
        var movies = await _movieRepository.FindAsync();
        var actualMovieId = movies.MaxBy(movieItem => movieItem.Id)!.Id++;

        return actualMovieId + 1;
    }

    private async Task ValidateIfMovieExist(string movieName)
    {
        var moviesMatches = await _movieRepository.FindAsync(movieName);
        if (moviesMatches.Any())
        {
            throw new MovieConflictException(ConstUtil.MoviesConflictAlreadyExistMessage);
        }
    }
}