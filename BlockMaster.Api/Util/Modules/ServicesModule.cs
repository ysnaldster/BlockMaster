using System.Diagnostics.CodeAnalysis;
using Autofac;
using BlockMaster.Business.Services;
using BlockMaster.Infrastructure.Repositories;

namespace BlockMaster.Api.Util.Modules;

[ExcludeFromCodeCoverage]
public class ServicesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .Register((context, _) => new MovieService(context.Resolve<MoviesRepository>(),
                context.Resolve<CacheRepository>()))
            .As<MovieService>()
            .SingleInstance();
    }
}