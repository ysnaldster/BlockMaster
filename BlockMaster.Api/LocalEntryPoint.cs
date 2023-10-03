using System.Diagnostics.CodeAnalysis;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Autofac.Extensions.DependencyInjection;
using BlockMaster.Domain.Util;

namespace BlockMaster.Api;

[ExcludeFromCodeCoverage]
public static class LocalEntryPoint
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        var awsConfigure = new AWSOptions
        {
            Region = RegionEndpoint.USEast1,
        };

        if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("IntegrationTestEnvironment")))
        {
            awsConfigure.DefaultClientConfig.ServiceURL = ConstUtil.AwsHost;
            awsConfigure.Credentials = new BasicAWSCredentials(ConstUtil.AwsAccessKey, ConstUtil.AwsSecretAccessKey);
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
                            .AddSystemsManager(ConstUtil.ParameterStorePath, awsConfigure)
                            .Build();
                        config.AddConfiguration(configurationBuilder);
                    });
                webBuilder.UseStartup<Startup>();
            });
    }
}