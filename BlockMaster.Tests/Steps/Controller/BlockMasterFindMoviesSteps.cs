using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BlockMaster.Domain.Entities;
using BlockMaster.Domain.Request.Identity;
using BlockMaster.Tests.Helpers;
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
    private TokenGenerationRequest _tokenRequest;
    private string _token;

    public BlockMasterFindMoviesSteps(AppFactoryFixture appFactoryFixture)
    {
        _httpClient = appFactoryFixture.CreateDefaultClient();
    }


    [Given(@"the data for create a token for find movies is")]
    public void GivenTheDataForCreateATokenForFindMoviesIs(Table table)
    {
        var tokenDetails = table.Rows.First();
        _tokenRequest = new TokenGenerationRequest
        {
            UserId = tokenDetails["UserId"],
            Email = tokenDetails["Email"],
            CustomClaims = new Dictionary<string, object>
            {
                { "admin", tokenDetails["CustomClaims"] }
            }
        };
    }

    [When(@"the token for find movies is created")]
    public async Task WhenTheTokenForFindMoviesIsCreated()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"block-master/v1/identity/token");
        var requestSerialize = JsonConvert.SerializeObject(_tokenRequest);
        request.Content = new StringContent(requestSerialize, Encoding.UTF8, "application/json");
        var response = await _httpClient.SendAsync(request);

        _token = IdentityHelper.ExtractToken(await response.Content.ReadAsStringAsync());
    }


    [When("movies are wanted")]
    public async Task WhenMoviesAreWanted()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"block-master/v1/movies");
        request.Headers.Authorization = new AuthenticationHeaderValue("bearer", _token);
        var response = await _httpClient.SendAsync(request);
        _responseMessage = response;
        var content = await response.Content.ReadAsStringAsync();
        if (_responseMessage.IsSuccessStatusCode)
        {
            _moviesMatches = JsonConvert.DeserializeObject<List<Movie>>(content);
        }
    }

    [Then("the movies returned by FindMovies are asserted")]
    public void ThenTheMoviesReturnedByFindMoviesAreAsserted()
    {
        _moviesMatches.Should().NotBeEmpty();
        _moviesMatches.Should().NotBeNull();
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