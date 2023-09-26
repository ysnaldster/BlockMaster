using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace BlockMaster.Tests;

[Binding]
public class UnitTest1
{
    [Given(@"the account id 9")]
    public void GivenTheEnabledParameterAsNull()
    {
    }

    [When(@"ip validation is enabled")]
    public async Task WhenApplicationsAreFound()
    {
    }

    [Then("the ip validation result should be 204")]
    public void Test1()
    {
        var test = "HolaMundo";
        test.Should().Be("HolaMundo");
    }
}