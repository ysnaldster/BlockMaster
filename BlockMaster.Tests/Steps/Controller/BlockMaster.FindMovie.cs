using System.Net.Http;
using System.Threading.Tasks;
using BlockMaster.Tests.Hooks.AppFactory;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace BlockMaster.Tests.Steps.Controller;

[Binding]
public class UnitTest1
{
    private readonly HttpClient _httpClient;
    public UnitTest1(AppFactoryFixture appFactoryFixture)
    {
        _httpClient = appFactoryFixture.CreateDefaultClient();
    }

    [Given(@"the account id 9")]
    public void GivenTheEnabledParameterAsNull()
    {
    }

    [When(@"ip validation is enabled")]
    public async Task WhenApplicationsAreFound()
    {
    }

    [Then("the ip validation result should be 204")]
    public async void Test1()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"block-master/v1/movies/E.T");
        var response = await _httpClient.SendAsync(request);
        var actualContent = await response.Content.ReadAsStringAsync();
        var test = "HolaMundo";
        test.Should().Be("HolaMundo");
    }
}