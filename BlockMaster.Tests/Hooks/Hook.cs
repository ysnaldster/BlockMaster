using System.Threading;
using System.Threading.Tasks;
using BlockMaster.Tests.Containers;
using TechTalk.SpecFlow;

namespace BlockMaster.Tests.Hooks;

[Binding]
public class Hook
{
    private static DynamoDbContainer _dynamoDbContainer;

    private static DynamoDbContainer DynamoDbContainer =>
        _dynamoDbContainer ?? new DynamoDbContainer();
    //private static ElastiCacheContainer ElastiCacheContainer { get; }
    //private static TestServerFixture TestServerFixture { get; }

    [BeforeTestRun]
    public static async Task BeforeTestRun()
    {
        ConfigureEnvironmentVariables();
        await DynamoDbContainer.InitializeAsync();
        //await ElastiCacheContainer.InitializeAsync();
        Thread.Sleep(20000);
    }

    [AfterTestRun]
    public static async Task AfterTestRun()
    {
        await DynamoDbContainer.DisposeAsync();
        //await ElastiCacheContainer.DisposeAsync();
    }

    private static void ConfigureEnvironmentVariables()
    {
    }
}