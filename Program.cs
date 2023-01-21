using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

builder.Host.ConfigureServices((context, services) =>
{
    services.AddRouteEndpoints(Assembly.GetExecutingAssembly());
    services.AddDbContext<IDbContext, ApplicationDbContext>(options =>
        options.UseSqlite(context.Configuration.GetConnectionString("Default")));
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouteEndpoints();

app.Run();