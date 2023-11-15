using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BlockMaster.Domain.Entities;
using BlockMaster.Domain.Request;
using BlockMaster.Domain.Request.Identity;
using BlockMaster.Tests.Helpers;
using BlockMaster.Tests.Hooks.AppFactory;
using FluentAssertions;
using Newtonsoft.Json;
using TechTalk.SpecFlow;

namespace BlockMaster.Tests.Steps.Controller;

[Binding]
public class BlockMasterCreateMovieSteps
{
    private readonly HttpClient _httpClient;
    private MovieRequest _movieToCreate;
    private Movie _movieCreated;
    private HttpResponseMessage _responseMessage;
    private TokenGenerationRequest _tokenRequest;
    private string _token;

    public BlockMasterCreateMovieSteps(AppFactoryFixture appFactoryFixture)
    {
        _httpClient = appFactoryFixture.CreateDefaultClient();
    }

    [Given(@"the data for create a token is")]
    public void GivenTheDataForCreateATokenIs(Table table)
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

    [Given("The details for creating the movie are")]
    public void GivenTheDetailsForCreatingTheMovieAre(Table table)
    {
        var movieDetails = table.Rows.First();
        _movieToCreate = new MovieRequest()
        {
            Name = movieDetails["Name"],
            Description = movieDetails["Description"],
            CountryCode = long.Parse(movieDetails["CountryCode"]),
            Score = double.Parse(movieDetails["Score"]),
            Category = movieDetails["Category"]
        };
    }

    [When(@"the token is created")]
    public async Task WhenTheTokenIsCreated()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"block-master/v1/identity/token");
        var requestSerialize = JsonConvert.SerializeObject(_tokenRequest);
        request.Content = new StringContent(requestSerialize, Encoding.UTF8, "application/json");
        var response = await _httpClient.SendAsync(request);

        _token = IdentityHelper.ExtractToken(await response.Content.ReadAsStringAsync());
    }

    [When("The movie is created")]
    public async Task WhenTheMovieIsCreated()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"block-master/v1/movies");
        var movieToSerialize = JsonConvert.SerializeObject(_movieToCreate);
        request.Content = new StringContent(movieToSerialize, Encoding.UTF8, "application/json");
        request.Headers.Authorization = new AuthenticationHeaderValue("bearer", _token);
        var response = await _httpClient.SendAsync(request);
        _responseMessage = response;
        var content = await _responseMessage.Content.ReadAsStringAsync();
        if (_responseMessage.IsSuccessStatusCode)
        {
            _movieCreated = JsonConvert.DeserializeObject<Movie>(content);
        }
    }

    [Then(@"the response should be (.*)")]
    public void ThenTheResponseShouldBe(int statusCode)
    {
        _responseMessage.StatusCode.Should().Be((HttpStatusCode)statusCode);
    }

    [Then(@"the result returned by CreateMovie is asserted")]
    public void ThenTheResultReturnedByCreateMovieIsAsserted()
    {
        _movieCreated.Should().NotBeNull();
        _movieCreated.Name.Should().Be(_movieToCreate.Name);
    }
}