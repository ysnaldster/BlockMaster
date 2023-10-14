using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BlockMaster.Domain.Entities;
using BlockMaster.Domain.Request;
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

    public BlockMasterCreateMovieSteps(AppFactoryFixture appFactoryFixture)
    {
        _httpClient = appFactoryFixture.CreateDefaultClient();
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

    [When("The movie is created")]
    public async Task WhenTheMovieIsCreated()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"block-master/v1/movies");
        var movieToSerialize = JsonConvert.SerializeObject(_movieToCreate);
        request.Content = new StringContent(movieToSerialize, Encoding.UTF8, "application/json");
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