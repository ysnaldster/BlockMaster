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
public class BlockMasterFindMovieSteps
{
    private readonly HttpClient _httpClient;
    private string _movieNameToFind;
    private HttpResponseMessage _responseMessage;
    private Movie _movieMatches;
    private TokenGenerationRequest _tokenRequest;
    private string _token;

    public BlockMasterFindMovieSteps(AppFactoryFixture appFactoryFixture)
    {
        _httpClient = appFactoryFixture.CreateDefaultClient();
    }

    [Given(@"the data for create a token for find movie is")]
    public void GivenTheDataForCreateATokenForFindMovieIs(Table table)
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

    [Given(@"the movie name is (.*)")]
    public void GivenTheMovieNameIs(string movieNameToFind)
    {
        _movieNameToFind = movieNameToFind;
    }

    [When(@"the token for find movie is created")]
    public async Task WhenTheTokenForFindMovieIsCreated()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"block-master/v1/identity/token");
        var requestSerialize = JsonConvert.SerializeObject(_tokenRequest);
        request.Content = new StringContent(requestSerialize, Encoding.UTF8, "application/json");
        var response = await _httpClient.SendAsync(request);

        _token = IdentityHelper.ExtractToken(await response.Content.ReadAsStringAsync());
    }

    [When(@"the movie is found")]
    public async Task WhenTheMovieIsFound()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"block-master/v1/movies/{_movieNameToFind}");
        request.Headers.Authorization = new AuthenticationHeaderValue("bearer", _token);
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