using System.Diagnostics.CodeAnalysis;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BlockMaster.Api.Util.Modules;

namespace BlockMaster.Api.Util;

[ExcludeFromCodeCoverage]
public static class IoCServiceCollection
{
    public static ContainerBuilder BuildContext(this IServiceCollection services, IConfiguration configuration)
    {
        var builder = new ContainerBuilder();
        builder.Populate(services);

        return BuilderContext(builder, configuration);
    }

    public static ContainerBuilder BuilderContext(this ContainerBuilder builder, IConfiguration configuration)
    {
        builder.RegisterModule(new ClientsModule(configuration));
        builder.RegisterModule(new RepositoriesModule(configuration));
        builder.RegisterModule(new ServicesModule(configuration));

        return builder;
    }
}