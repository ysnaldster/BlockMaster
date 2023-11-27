using System.Diagnostics.CodeAnalysis;
using Amazon;
using Amazon.DynamoDBv2;
using Autofac;
using BlockMaster.Infrastructure.Clients;

namespace BlockMaster.Api.Util.Modules;

[ExcludeFromCodeCoverage]
public class ClientsModule : Module
{
    private readonly IConfiguration _configuration;

    public ClientsModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder
            .Register((_, _) => new AmazonDynamoDBClient(RegionEndpoint.USEast1))
            .As<IAmazonDynamoDB>()
            .SingleInstance();
        builder
            .Register((_, _) => new ElastiCacheClient(_configuration.GetValue<string>("RedisEndpoint")))
            .As<ElastiCacheClient>()
            .SingleInstance();
    }
}