using Autofac.Extensions.DependencyInjection;

namespace BlockMaster.Api;

public class LocalEntryPoint
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureServices(services => { services.AddAutofac(); })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .ConfigureAppConfiguration((_, config) =>
                    {
                        var configurationBuilder = new ConfigurationBuilder()
                            .AddSystemsManager(Environment.GetEnvironmentVariable("PARAMETER_STORE_PATH"))
                            .Build();
                        config.AddConfiguration(configurationBuilder);
                    });
                webBuilder.UseStartup<Startup>();
            });
}