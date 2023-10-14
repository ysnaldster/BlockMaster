using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BlockMaster.Domain.Entities;
using BlockMaster.Tests.Configuration;
using BlockMaster.Tests.Extensions;
using BlockMaster.Tests.Hooks.AppFactory;
using FluentAssertions;
using Newtonsoft.Json;
using TechTalk.SpecFlow;

namespace BlockMaster.Tests.Steps.Controller;

[Binding]
public class BlockMasterFindMovieSteps : TestExtensions
{
    private readonly HttpClient _httpClient;
    private string _movieNameToFind;
    private HttpResponseMessage _responseMessage;
    private List<Movie> _moviesMatches;
    private Movie _movieFound;
    private Movie _movieComparative;

    public BlockMasterFindMovieSteps(AppFactoryFixture appFactoryFixture)
    {
        _httpClient = appFactoryFixture.CreateDefaultClient();
    }

    [Given(@"the movie name is (.*)")]
    public void GivenMovieName(string movieNameToFind)
    {
        _movieNameToFind = movieNameToFind;
    }

    [When(@"the movie is found")]
    public async Task WhenTheMovieIsFound()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"block-master/v1/movies/{_movieNameToFind}");
        var response = await _httpClient.SendAsync(request);
        _responseMessage = response;
        //Pendiente por desacoplar
        var content = await response.Content.ReadAsStringAsync();
        _moviesMatches = JsonConvert.DeserializeObject<List<Movie>>(content);
        _movieComparative = await GetMovieFromStreamReader("../../../Util/JsonFiles/GetMovie.json");
        _movieFound = _moviesMatches.SingleOrDefault();
    }

    [Then("the movie is contain in the list")]
    public void TheTheReturnedContainTheMovieInTheList()
    {
        var isContain = _moviesMatches.Any(movie => movie.Id == _movieComparative.Id);
        isContain.Should().Be(true);
    }

    [Then("the movie name should be (.*)")]
    public void ThenTheReturnedMovieNameShouldBe(string movieName)
    {
        _movieFound.Name.Should().Be(movieName);
    }

    [Then("the http status code should be Ok")]
    public void ThenHttpStatusCodeShouldBeOk()
    {
        _responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Then("the http status code should be notFound")]
    public void ThenHttpStatusCodeShouldBeNotFound()
    {
        _responseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}