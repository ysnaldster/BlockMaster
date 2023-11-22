using System.Diagnostics.CodeAnalysis;
using Autofac;
using BlockMaster.Business.Services;
using BlockMaster.Domain.Repositories;
using BlockMaster.Domain.Services;
using BlockMaster.Infrastructure.Repositories;

namespace BlockMaster.Api.Util.Modules;

[ExcludeFromCodeCoverage]
public class ServicesModule : Module
{
    private readonly IConfiguration _configuration;

    public ServicesModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder
            .Register((context, _) => new MovieService(context.Resolve<IMovieRepository>(),
                context.Resolve<ICacheMovieRepository>()))
            .As<IMovieService>()
            .SingleInstance();
        builder
            .Register((_, _) => new GenerateTokenService(_configuration.GetValue<string>("ApiKey")))
            .As<IGenerateTokenService>()
            .SingleInstance();
    }
}