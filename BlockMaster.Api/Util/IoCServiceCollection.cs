using Autofac;
using Autofac.Extensions.DependencyInjection;
using BlockMaster.Api.Util.Modules;

namespace BlockMaster.Api.Util;

public static class IoCServiceCollection
{
    #region public methods

    public static ContainerBuilder BuildContext(this IServiceCollection services, IConfiguration configuration)
    {
        var builder = new ContainerBuilder();
        builder.Populate(services);
        return BuilderContext(builder, configuration);
    }

    public static ContainerBuilder BuilderContext(this ContainerBuilder builder, IConfiguration configuration)
    {
        builder.RegisterModule(new ClientsModule());
        builder.RegisterModule(new RepositoriesModule(configuration));
        builder.RegisterModule(new ServicesModule());
        return builder;
    }

    #endregion
}