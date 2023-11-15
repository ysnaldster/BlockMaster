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
public class BlockMasterUpdateMovieSteps
{
    private readonly HttpClient _httpClient;
    private Movie _movieUpdated;
    private MovieRequest _movieForUpdate;
    private string _movieNameToUpdate;
    private HttpResponseMessage _responseMessage;
    private TokenGenerationRequest _tokenRequest;
    private string _token;

    public BlockMasterUpdateMovieSteps(AppFactoryFixture appFactoryFixture)
    {
        _httpClient = appFactoryFixture.CreateDefaultClient();
    }

    [Given(@"the data for create a token for update is")]
    public void GivenTheDataForCreateATokenForUpdateIs(Table table)
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

    [Given("the movie name for update is (.*)")]
    public void GivenTheMovieNameForUpdateIs(string movieName)
    {
        _movieNameToUpdate = movieName;
    }

    [Given("The details for updating the movie are")]
    public void GivenTheDetailsForUpdatingTheMovieAre(Table table)
    {
        var movieDetails = table.Rows.First();
        _movieForUpdate = new MovieRequest()
        {
            Name = movieDetails["Name"],
            Description = movieDetails["Description"],
            CountryCode = long.Parse(movieDetails["CountryCode"]),
            Score = double.Parse(movieDetails["Score"]),
            Category = movieDetails["Category"]
        };
    }

    [When(@"the token for update is created")]
    public async Task WhenTheTokenForUpdateIsCreated()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"block-master/v1/identity/token");
        var requestSerialize = JsonConvert.SerializeObject(_tokenRequest);
        request.Content = new StringContent(requestSerialize, Encoding.UTF8, "application/json");
        var response = await _httpClient.SendAsync(request);

        _token = IdentityHelper.ExtractToken(await response.Content.ReadAsStringAsync());
    }

    [When("The movie is updated")]
    public async Task WhenTheMovieIsUpdated()
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"block-master/v1/movies/{_movieNameToUpdate}");
        var movieToSerialize = JsonConvert.SerializeObject(_movieForUpdate);
        request.Headers.Authorization = new AuthenticationHeaderValue("bearer", _token);
        request.Content = new StringContent(movieToSerialize, Encoding.UTF8, "application/json");
        var response = await _httpClient.SendAsync(request);
        _responseMessage = response;
        var content = await _responseMessage.Content.ReadAsStringAsync();
        if (_responseMessage.IsSuccessStatusCode)
        {
            _movieUpdated = JsonConvert.DeserializeObject<Movie>(content);
        }
    }

    [Then(@"the response for UpdateMovie should be (.*)")]
    public void ThenTheResponseForUpdateMovieShouldBe(int statusCode)
    {
        _responseMessage.StatusCode.Should().Be((HttpStatusCode)statusCode);
    }

    [Then(@"the result returned by UpdateMovie is asserted")]
    public void ThenTheResultReturnedByUpdateMovieIsAsserted()
    {
        _movieUpdated.Should().NotBeNull();
        _movieUpdated.Name.Should().Be(_movieForUpdate.Name);
    }
}