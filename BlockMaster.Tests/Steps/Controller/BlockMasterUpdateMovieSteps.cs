using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BlockMaster.Domain.Entities;
using BlockMaster.Domain.Request;
using BlockMaster.Tests.Hooks.AppFactory;
using BlockMaster.Tests.Util;
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

    public BlockMasterUpdateMovieSteps(AppFactoryFixture appFactoryFixture)
    {
        _httpClient = appFactoryFixture.CreateDefaultClient();
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

    [When("The movie is updated")]
    public async Task WhenTheMovieIsUpdated()
    {
        var token = TokenUtils.GetToken();
        var request = new HttpRequestMessage(HttpMethod.Put, $"block-master/v1/movies/{_movieNameToUpdate}");
        var movieToSerialize = JsonConvert.SerializeObject(_movieForUpdate);
        request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
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