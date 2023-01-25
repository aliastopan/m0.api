namespace m0.api.Routes;

public class GasRecordEndpoint : IRouteEndpoint
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost("/mq3", MQ3Reading);
    }

    internal IResult MQ3Reading(MQ3Request request, IDbContext dbContext)
    {
        var gasRecord = new GasRecord
        {
            Id = Guid.NewGuid(),
            DateTime = DateTime.Now,
            Concentration = request.AnalogValue
        };

        dbContext.GasRecords.Add(gasRecord);
        dbContext.Commit();

        return Results.Ok();
    }
}
