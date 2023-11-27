using System.Diagnostics.CodeAnalysis;
using Amazon.DynamoDBv2;
using Autofac;
using BlockMaster.Domain.Repositories;
using BlockMaster.Infrastructure.Clients;
using BlockMaster.Infrastructure.Repositories;

namespace BlockMaster.Api.Util.Modules;

[ExcludeFromCodeCoverage]
public class RepositoriesModule : Module
{
    private readonly IConfiguration _configuration;

    public RepositoriesModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder
            .Register((context, _) => new MoviesRepository(context.Resolve<IAmazonDynamoDB>(),
                _configuration.GetValue<string>("DynamoDbMoviesTableName")))
            .As<IMovieRepository>()
            .SingleInstance();
        builder
            .Register((context, _) => new CacheMovieRepository(context.Resolve<ElastiCacheClient>()))
            .As<ICacheMovieRepository>()
            .SingleInstance();
    }
}