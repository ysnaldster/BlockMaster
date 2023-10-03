using System.Diagnostics.CodeAnalysis;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Autofac;
using BlockMaster.Api;
using BlockMaster.Tests.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlockMaster.Tests.Hooks.AppFactory;

[ExcludeFromCodeCoverage]
public class AppFactoryFixture : WebApplicationFactory<Startup>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(serviceCollection =>
        {
            serviceCollection.AddDefaultAWSOptions(new AWSOptions
            {
                Region = RegionEndpoint.GetBySystemName(ConstUtil.AwsRegion),
                Credentials = new BasicAWSCredentials(ConstUtil.AwsAccessKey, ConstUtil.AwsSecretAccessKey),
                DefaultClientConfig = { ServiceURL = ConstUtil.AwsHost }
            });
        });

        base.ConfigureWebHost(builder);

        builder.ConfigureTestContainer<ContainerBuilder>(containerBuilder =>
        {
            containerBuilder
                .Register(((_, _) => new AmazonDynamoDBClient(ConstUtil.AwsAccessKey, ConstUtil.AwsSecretAccessKey,
                    new AmazonDynamoDBConfig
                    {
                        RegionEndpoint = RegionEndpoint.GetBySystemName(ConstUtil.AwsRegion),
                        ServiceURL = ConstUtil.AwsHost
                    })))
                .As<IAmazonDynamoDB>()
                .SingleInstance();
        });

        base.ConfigureWebHost(builder);
    }
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseServiceProviderFactory(new CustomServiceProviderFactory());

        return base.CreateHost(builder);
    }
}