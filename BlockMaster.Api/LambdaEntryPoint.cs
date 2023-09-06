using Autofac.Extensions.DependencyInjection;

namespace BlockMaster.Api;

/// BlockMaster.Api::BlockMaster.Api.LambdaEntryPoint::FunctionHandlerAsync
public class LambdaEntryPoint : Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
{
    protected override void Init(IWebHostBuilder builder)
    {
        builder
            .ConfigureAppConfiguration((_, config) =>
            {
                var configurationBuilder = new ConfigurationBuilder()
                    .AddSystemsManager(Environment.GetEnvironmentVariable("PARAMETER_STORE_PATH"))
                    .Build();
                config.AddConfiguration(configurationBuilder);
            })
            .UseStartup<Startup>();
    }

    protected override void Init(IHostBuilder builder)
    {
        builder.UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureServices((_, services) => { services.AddAutofac(); });
    }
}