using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BlockMaster.Domain.Entities;
using BlockMaster.Tests.Hooks.AppFactory;
using FluentAssertions;
using Newtonsoft.Json;
using TechTalk.SpecFlow;

namespace BlockMaster.Tests.Steps.Controller;

[Binding]
public class BlockMasterFindMovieSteps
{
    private readonly HttpClient _httpClient;
    private string _movieNameToFind;
    private HttpResponseMessage _responseMessage;
    private List<Movie> _moviesMatches;
    private Movie _movieFound;

    public BlockMasterFindMovieSteps(AppFactoryFixture appFactoryFixture)
    {
        _httpClient = appFactoryFixture.CreateDefaultClient();
    }

    [Given(@"the movie name is (.*)")]
    public void GivenTheMovieNameIs(string movieNameToFind)
    {
        _movieNameToFind = movieNameToFind;
    }

    [When(@"the movie is found")]
    public async Task WhenTheMovieIsFound()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"block-master/v1/movies/{_movieNameToFind}");
        var response = await _httpClient.SendAsync(request);
        _responseMessage = response;
        var content = await response.Content.ReadAsStringAsync();
        if (_responseMessage.IsSuccessStatusCode)
        {
            _moviesMatches = JsonConvert.DeserializeObject<List<Movie>>(content);
            _movieFound = _moviesMatches.Find(movie => movie.Name == _movieNameToFind);
        }
    }

    [Then("the movie name should be (.*)")]
    public void ThenTheMovieNameShouldBe(string movieName)
    {
        _movieFound.Name.Should().Be(movieName);
    }

    [Then("the result should be (.*)")]
    public void ThenTheResultShouldBe(int codeResult)
    {
        _responseMessage.StatusCode.Should().Be((HttpStatusCode)codeResult);
    }
}