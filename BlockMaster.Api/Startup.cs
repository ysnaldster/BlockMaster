using System.Diagnostics.CodeAnalysis;
using Autofac;
using BlockMaster.Api.Extensions;
using BlockMaster.Api.Middleware;
using BlockMaster.Api.Util;
using BlockMaster.Domain.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BlockMaster.Api;

[ExcludeFromCodeCoverage]
public class Startup
{
    private IConfiguration Configuration { get; set; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(o => o.AddPolicy(ConstUtil.AllowCorsPolicy, builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }));

        services.SetAuthenticationStructure(Configuration);
        services.SetAuthorizationStructure();
        services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
        services.AddControllersWithViews(options =>
            options.UseGeneralRoutePrefix("block-master/v{version:apiVersion}"));
        services.AddApiVersioning(options => options.ReportApiVersions = true);
        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        services.ConfigureApiVersioning();
        services.AddHealthChecks();
        services.ConfigureSwagger();
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.BuilderContext(Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            
        }
        
        app.BuildSwagger();
        
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors(ConstUtil.AllowCorsPolicy);

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}