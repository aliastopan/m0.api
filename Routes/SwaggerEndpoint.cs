namespace m0.api.Routes;

public class SwaggerEndpoint : IRouteEndpoint, IRouteService
{
    public void DefineEndpoints(WebApplication app)
    {
        if(app.Environment.IsProduction())
        {
            return;
        }

        app.UseSwagger();
        app.UseSwaggerUI();
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}
