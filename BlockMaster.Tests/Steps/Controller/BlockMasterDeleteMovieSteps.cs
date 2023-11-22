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
public class BlockMasterDeleteMovieSteps
{
    private readonly HttpClient _httpClient;
    private HttpResponseMessage _responseMessage;
    private Movie _movieDeleted;
    private Movie _movieDeletedExpected;
    private string _movieToDelete;

    public BlockMasterDeleteMovieSteps(AppFactoryFixture appFactoryFixture)
    {
        _httpClient = appFactoryFixture.CreateDefaultClient();
    }

    [Given("the movie name for deleted is (.*)")]
    public void GivenTheMovieNameForDeletedIs(string movieToDelete)
    {
        _movieToDelete = movieToDelete;
    }

    [When("the movie is deleted")]
    public async Task WhenTheMovieIsDeleted()
    {
        var token = TokenUtils.GetToken();
        var request = new HttpRequestMessage(HttpMethod.Delete, $"block-master/v1/movies/{_movieToDelete}");
        request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
        var response = await _httpClient.SendAsync(request);
        _responseMessage = response;
        var content = await response.Content.ReadAsStringAsync();
        if (_responseMessage.IsSuccessStatusCode)
        {
            _movieDeleted = JsonConvert.DeserializeObject<Movie>(content);
            _movieDeletedExpected =
                (await MoviesUtil.GetMoviesFromStreamReader("../../../Util/JsonFiles/GetMovies.json"))
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