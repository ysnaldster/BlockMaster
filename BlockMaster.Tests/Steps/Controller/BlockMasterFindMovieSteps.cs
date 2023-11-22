using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BlockMaster.Domain.Entities;
using BlockMaster.Tests.Hooks.AppFactory;
using BlockMaster.Tests.Util;
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
    private Movie _movieMatches;

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
        var token = TokenUtils.GetToken();
        var request = new HttpRequestMessage(HttpMethod.Get, $"block-master/v1/movies/{_movieNameToFind}");
        request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
        var response = await _httpClient.SendAsync(request);
        _responseMessage = response;
        var content = await response.Content.ReadAsStringAsync();
        if (_responseMessage.IsSuccessStatusCode)
        {
            _movieMatches = JsonConvert.DeserializeObject<Movie>(content);
        }
    }

    [Then("the movie returned by FindMovie is asserted")]
    public void ThenTheMovieReturnedByFindMovieIsAsserted()
    {
        _movieMatches.Should().NotBeNull();
        _movieMatches.Name.Should().Be(_movieNameToFind);
    }

    [Then("the result should be (.*)")]
    public void ThenTheResultShouldBe(int codeResult)
    {
        _responseMessage.StatusCode.Should().Be((HttpStatusCode)codeResult);
    }
}