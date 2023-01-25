namespace m0.api.Routes;

public class GasRecordEndpoint : IRouteEndpoint
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost("/mq3", MQ3Write);
        app.MapGet("/mq3/read", MQ3Read);
    }

    internal IResult MQ3Write(MQ3WriteRequest request, IDbContext dbContext)
    {
        var gasRecord = new GasRecord
        {
            Id = Guid.NewGuid(),
            DateTime = DateTime.Now,
            Concentration = request.AnalogValue
        };

        dbContext.GasRecords.Add(gasRecord);
        dbContext.Commit();

        return Results.Ok(gasRecord);
    }

    internal IResult MQ3Read(IDbContext dbContext)
    {
        var gasRecords = dbContext.GasRecords.Take(10);

        return Results.Ok(gasRecords);
    }
}
