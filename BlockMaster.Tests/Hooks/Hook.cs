using System;
using System.Threading.Tasks;
using BlockMaster.Tests.Containers;
using BlockMaster.Tests.Hooks.AppFactory;
using TechTalk.SpecFlow;

namespace BlockMaster.Tests.Hooks;

[Binding]
public class Hook
{
    private static DynamoDbContainer _dynamoDbContainer;
    private static AppFactoryFixture _appFactoryFixture;
    private static ElastiCacheContainer _elastiCacheContainer;

    private static DynamoDbContainer DynamoDbContainer =>
        _dynamoDbContainer ?? new DynamoDbContainer();

    private static AppFactoryFixture AppFactoryFixture =>
        _appFactoryFixture ?? new AppFactoryFixture();

    private static ElastiCacheContainer ElastiCacheContainer =>
        _elastiCacheContainer ?? new ElastiCacheContainer();
    

    [BeforeTestRun]
    public static async Task BeforeTestRun()
    {
        Environment.SetEnvironmentVariable("IntegrationTestEnvironment", "IntegrationTest");
        await DynamoDbContainer.InitializeAsync();
        await DynamoDbContainer.PopulateDynamoDb();
        await ElastiCacheContainer.InitializeAsync();
    }

    [AfterTestRun]
    public static async Task AfterTestRun()
    {
        await DynamoDbContainer.DisposeAsync();
        await AppFactoryFixture.DisposeAsync();
        await ElastiCacheContainer.DisposeAsync();
    }
}