using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BlockMaster.Tests.Hooks.AppFactory;

[ExcludeFromCodeCoverage]
public class CustomServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
{
    private readonly AutofacServiceProviderFactory
        _wrapped = new();

    private IServiceCollection _services;

    public ContainerBuilder CreateBuilder(IServiceCollection services)
    {
        _services = services;
        
        return _wrapped.CreateBuilder(services);
    }

    public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
    {
        var serviceProvider =
            _services.BuildServiceProvider();
        var configureContainerFilters = serviceProvider
            .GetRequiredService<
                IEnumerable<IStartupConfigureContainerFilter<ContainerBuilder>>>();

        foreach (var filter in configureContainerFilters)
        {
            filter.ConfigureContainer(_ => { })(containerBuilder);
        }

        return _wrapped.CreateServiceProvider(containerBuilder);
    }
}