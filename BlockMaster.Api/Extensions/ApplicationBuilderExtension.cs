namespace BlockMaster.Api.Extensions;

public static class ApplicationBuilderExtension
{
    public static void BuildSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger(swaggerOptions =>
        {
            swaggerOptions.RouteTemplate = "block-master/swagger/{documentname}/swagger.json";
        });        
        app.UseSwaggerUI(swaggerUiOptions =>
        {
            swaggerUiOptions.SwaggerEndpoint("/block-master/swagger/v1/swagger.json", "v1");
            swaggerUiOptions.RoutePrefix = "block-master/swagger";
        });
    }
}