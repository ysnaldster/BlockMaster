using System.Diagnostics.CodeAnalysis;
using Autofac;
using BlockMaster.Api.Extensions;
using BlockMaster.Api.Middleware;
using BlockMaster.Api.Util;
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
        services.AddCors(o => o.AddPolicy("AllowCorsPolicy", builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }));

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

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors("AllowCorsPolicy");

        app.UseAuthorization();

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}