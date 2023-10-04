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
public class BlockMasterFindMoviesSteps
{
    private readonly HttpClient _httpClient;
    private HttpResponseMessage _responseMessage;
    private List<Movie> _moviesMatches;

    public BlockMasterFindMoviesSteps(AppFactoryFixture appFactoryFixture)
    {
        _httpClient = appFactoryFixture.CreateDefaultClient();
    }

    [When("movies are wanted")]
    public async Task WhenMoviesAreWanted()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"block-master/v1/movies");
        var response = await _httpClient.SendAsync(request);
        _responseMessage = response;
        var content = await response.Content.ReadAsStringAsync();
        if (_responseMessage.IsSuccessStatusCode)
        {
            _moviesMatches = JsonConvert.DeserializeObject<List<Movie>>(content);
        }
    }

    [Then("the http status code should be (.*)")]
    public void ThenTheHttpStatusCodeShouldBe(int codeResult)
    {
        _responseMessage.StatusCode.Should().Be((HttpStatusCode)codeResult);
    }

    [Then("the movies count is equal (.*)")]
    public void ThenTheMoviesCountIsEqual(int count)
    {
        _moviesMatches.Count.Should().Be(count);
    }
}