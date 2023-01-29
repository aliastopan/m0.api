namespace m0.api.Routes;

public class GasRecordEndpoint : IRouteEndpoint
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost("/api", Write);
        app.MapGet("/api/read", Read);
        app.MapGet("/api/read/today", ReadToday);
        app.MapGet("/api/read/date", ReadAtDate);
    }

    internal IResult Write(WriteRequest request, IDbContext dbContext)
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

    internal IResult Read(IDbContext dbContext)
    {
        var gasRecords = dbContext.GasRecords
            .OrderByDescending(x => x.DateTime)
            .Take(10);

        return Results.Ok(gasRecords);
    }

    internal IResult ReadToday(IDbContext dbContext)
    {
        var gasRecords = dbContext.GasRecords
            .Where(x => x.DateTime.IsToday())
            .OrderByDescending(x => x.DateTime);

        return Results.Ok(gasRecords);
    }

    internal IResult ReadAtDate(ReadAtDateRequest request, IDbContext dbContext)
    {
        var gasRecords = dbContext.GasRecords
            .Where(x => x.DateTime.AtDate(request.Date))
            .OrderByDescending(x => x.DateTime);

        return Results.Ok(gasRecords);
    }
}
