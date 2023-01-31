using Microsoft.AspNetCore.Mvc;

namespace m0.api.Routes;

public class GasRecordEndpoint : IRouteEndpoint
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost("/api/write", Write);
        app.MapPost("/api/write/realtime", WriteRealtime);
        app.MapGet("/api/read", Read);
        app.MapGet("/api/read/realtime", ReadRealtime);
        app.MapGet("/api/read/today", ReadToday);
        app.MapGet("/api/read/date", ReadAtDate);
    }

    internal IResult WriteRealtime([FromBody] WriteRequest request)
    {
        m0.api.Realtime.Mq = request.AnalogValue;
        return Results.Ok();
    }

    internal IResult Write(
        [FromBody] WriteRequest request,
        [FromServices] IDbContext dbContext)
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

    internal IResult ReadRealtime()
    {
        return Results.Ok(m0.api.Realtime.Mq);
    }

    internal IResult Read([FromServices] IDbContext dbContext)
    {
        var gasRecords = dbContext.GasRecords
            .OrderByDescending(x => x.DateTime)
            .Take(10);

        return Results.Ok(gasRecords);
    }

    internal IResult ReadToday([FromServices] IDbContext dbContext)
    {
        var gasRecords = dbContext.GasRecords
            .Where(x =>
                x.DateTime.Day == DateTime.Today.Day &&
                x.DateTime.Month == DateTime.Today.Month &&
                x.DateTime.Year == DateTime.Today.Year)
            .OrderByDescending(x => x.DateTime);

        return Results.Ok(gasRecords);
    }

    internal IResult ReadAtDate(
        [FromBody] ReadAtDateRequest request,
        [FromServices] IDbContext dbContext)
    {
        var gasRecords = dbContext.GasRecords
            .Where(x =>
                x.DateTime.Day == request.Date.Day &&
                x.DateTime.Month == request.Date.Month &&
                x.DateTime.Year == request.Date.Year)
            .OrderByDescending(x => x.DateTime);

        return Results.Ok(gasRecords);
    }
}
