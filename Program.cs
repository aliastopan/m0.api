using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System.Reflection;
using m0.api;

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
        options.UseSqlite(context.Configuration
            .GetConnectionString("Default")));

    services.Configure<Settings>(context.Configuration
        .GetSection(Settings.SectionName));
});

builder.Services.AddHostedService<HttpClientService>();
builder.Services.AddCors(opt =>
{
    var corsBuilder = new CorsPolicyBuilder();
    corsBuilder.AllowAnyHeader();
    corsBuilder.AllowAnyMethod();
    corsBuilder.AllowAnyOrigin();
    opt.AddPolicy("OpenCorsPolicy", corsBuilder.Build());
});

var corsBuilder = new CorsPolicyBuilder();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouteEndpoints();
app.UseCors("OpenCorsPolicy");

app.Run();