using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BlockMaster.Domain.Entities;
using BlockMaster.Domain.Request.Identity;
using BlockMaster.Tests.Extensions;
using BlockMaster.Tests.Helpers;
using BlockMaster.Tests.Hooks.AppFactory;
using FluentAssertions;
using Newtonsoft.Json;
using TechTalk.SpecFlow;

namespace BlockMaster.Tests.Steps.Controller;

[Binding]
public class BlockMasterDeleteMovieSteps : TestExtensions
{
    private readonly HttpClient _httpClient;
    private HttpResponseMessage _responseMessage;
    private Movie _movieDeleted;
    private Movie _movieDeletedExpected;
    private string _movieToDelete;
    private TokenGenerationRequest _tokenRequest;
    private string _token;

    public BlockMasterDeleteMovieSteps(AppFactoryFixture appFactoryFixture)
    {
        _httpClient = appFactoryFixture.CreateDefaultClient();
    }

    [Given(@"the data for create a token for delete movie is")]
    public void GivenTheDataForCreateATokenForDeleteMovieIs(Table table)
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

    [Given("the movie name for deleted is (.*)")]
    public void GivenTheMovieNameForDeletedIs(string movieToDelete)
    {
        _movieToDelete = movieToDelete;
    }

    [When(@"the token for delete a movie is created")]
    public async Task WhenTheTokenForDeleteAMovieIsCreated()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"block-master/v1/identity/token");
        var requestSerialize = JsonConvert.SerializeObject(_tokenRequest);
        request.Content = new StringContent(requestSerialize, Encoding.UTF8, "application/json");
        var response = await _httpClient.SendAsync(request);

        _token = IdentityHelper.ExtractToken(await response.Content.ReadAsStringAsync());
    }

    [When("the movie is deleted")]
    public async Task WhenTheMovieIsDeleted()
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"block-master/v1/movies/{_movieToDelete}");
        request.Headers.Authorization = new AuthenticationHeaderValue("bearer", _token);
        var response = await _httpClient.SendAsync(request);
        _responseMessage = response;
        var content = await response.Content.ReadAsStringAsync();
        if (_responseMessage.IsSuccessStatusCode)
        {
            _movieDeleted = JsonConvert.DeserializeObject<Movie>(content);
            _movieDeletedExpected = (await GetMoviesFromStreamReader("../../../Util/JsonFiles/GetMovies.json"))
                .Find(movie => movie.Name == _movieToDelete);
        }
    }

    [Then("the movie deleted is equal to expected")]
    public void ThenTheMovieDeletedIsEqualToExpected()
    {
        _movieDeleted.Should().BeEquivalentTo(_movieDeletedExpected);
    }

    [Then("the result for http response should be (.*)")]
    public void ThenTheResultForHttpResponseShouldBe(int codeResult)
    {
        _responseMessage.StatusCode.Should().Be((HttpStatusCode)codeResult);
    }
}