using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlockMaster.Tests.Containers;
using BlockMaster.Tests.Extensions;
using BlockMaster.Tests.Hooks.AppFactory;
using BlockMaster.Tests.Util;
using TechTalk.SpecFlow;

namespace BlockMaster.Tests.Hooks;

[Binding]
public class Hook
{
    private static LocalStackContainer _localStackContainer;
    private static AppFactoryFixture _appFactoryFixture;
    private static ElastiCacheContainer _elastiCacheContainer;
    private static HttpClient _httpClient;

    private static LocalStackContainer LocalStackContainer =>
        _localStackContainer ??= new LocalStackContainer();

    private static AppFactoryFixture AppFactoryFixture =>
        _appFactoryFixture ??= new AppFactoryFixture();

    private static ElastiCacheContainer ElastiCacheContainer =>
        _elastiCacheContainer ??= new ElastiCacheContainer();

    private static HttpClient HttpClient =>
        _httpClient ??= AppFactoryFixture.CreateDefaultClient();

    [BeforeTestRun]
    public static async Task BeforeTestRun()
    {
        Environment.SetEnvironmentVariable("IntegrationTestEnvironment", "IntegrationTest");
        await LocalStackContainer.InitializeAsync();
        await ElastiCacheContainer.InitializeAsync();
        await GenerateAuthenticationToken();
    }

    [AfterTestRun]
    public static async Task AfterTestRun()
    {
        await LocalStackContainer.DisposeAsync();
        await AppFactoryFixture.DisposeAsync();
        await ElastiCacheContainer.DisposeAsync();
    }

    [BeforeScenario]
    public static async Task BeforeScenarioRun()
    {
        await LocalStackContainer.PopulateDynamoDb();
    }

    [AfterScenario]
    public static async Task AfterScenarioRun()
    {
        await LocalStackContainer.ClearDynamoDb();
    }

    private static async Task GenerateAuthenticationToken()
    {
        var token = await HttpClient.GenerateAuthenticationToken();
        TokenUtils.SetToken(token);
    }
}