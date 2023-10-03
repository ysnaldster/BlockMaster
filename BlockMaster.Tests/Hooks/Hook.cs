using System;
using System.Threading.Tasks;
using BlockMaster.Tests.Containers;
using BlockMaster.Tests.Hooks.AppFactory;
using TechTalk.SpecFlow;

namespace BlockMaster.Tests.Hooks;

[Binding]
public class Hook
{
    private static LocalStackContainer _localStackContainer;
    private static AppFactoryFixture _appFactoryFixture;
    private static ElastiCacheContainer _elastiCacheContainer;

    private static LocalStackContainer LocalStackContainer =>
        _localStackContainer ??= new LocalStackContainer();

    private static AppFactoryFixture AppFactoryFixture =>
        _appFactoryFixture ??= new AppFactoryFixture();

    private static ElastiCacheContainer ElastiCacheContainer =>
        _elastiCacheContainer ??= new ElastiCacheContainer();


    [BeforeTestRun]
    public static async Task BeforeTestRun()
    {
        Environment.SetEnvironmentVariable("IntegrationTestEnvironment", "IntegrationTest");
        await LocalStackContainer.InitializeAsync();
        await LocalStackContainer.PopulateDynamoDb();
        await ElastiCacheContainer.InitializeAsync();
    }

    [AfterTestRun]
    public static async Task AfterTestRun()
    {
        await LocalStackContainer.DisposeAsync();
        await AppFactoryFixture.DisposeAsync();
        await ElastiCacheContainer.DisposeAsync();
    }
}