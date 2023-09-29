using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Autofac.Extensions.DependencyInjection;

namespace BlockMaster.Api;

public class LocalEntryPoint
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        var awsConfigure = new AWSOptions
        {
            Region = RegionEndpoint.USEast1,
        };

        if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("IntegrationTestEnvironment")))
        {
            awsConfigure.DefaultClientConfig.ServiceURL = "http://localhost:4566";
            awsConfigure.Credentials = new BasicAWSCredentials("123", "123");
        }

        return Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureServices(services => { services.AddAutofac(); })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .ConfigureAppConfiguration((_, config) =>
                    {
                        var configurationBuilder = new ConfigurationBuilder()
                            .AddSystemsManager("/BlockMaster/", awsConfigure)
                            .Build();
                        config.AddConfiguration(configurationBuilder);
                    });
                webBuilder.UseStartup<Startup>();
            });
    }
}