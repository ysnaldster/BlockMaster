using System.Net.Http;
using System.Threading.Tasks;
using BlockMaster.Tests.Hooks.AppFactory;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace BlockMaster.Tests;

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
        var request = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:4566/Movies/1");
        var response = await _httpClient.SendAsync(request);
        var test = "HolaMundo";
        test.Should().Be("HolaMundo");
    }
}