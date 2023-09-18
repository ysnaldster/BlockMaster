﻿using System.Diagnostics.CodeAnalysis;
using Amazon.DynamoDBv2;
using Autofac;
using BlockMaster.Infrastructure.Clients;
using BlockMaster.Infrastructure.Repositories;

namespace BlockMaster.Api.Util.Modules;

[ExcludeFromCodeCoverage]
public class RepositoriesModule : Module
{
    #region private attributes

    private readonly IConfiguration _configuration;

    #endregion

    #region public methods

    public RepositoriesModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion


    #region protected methods

    protected override void Load(ContainerBuilder builder)
    {
        builder
            .Register((context, _) => new MoviesRepository(context.Resolve<IAmazonDynamoDB>(),
                _configuration.GetValue<string>("DynamoDbMoviesTableName")))
            .As<MoviesRepository>()
            .SingleInstance();
        //Se debe de generar el parametro de ElastiCache con CDK, y crear la instancia de ElastiCache
        builder
            .Register((context, _) => new CacheRepository(context.Resolve<ElastiCacheClient>()))
            .As<CacheRepository>()
            .SingleInstance();
    }

    #endregion
}