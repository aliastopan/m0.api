using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureServices((context, services) =>
{
    services.AddRouteEndpoints(Assembly.GetExecutingAssembly());
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(context.Configuration.GetConnectionString("Default")));
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouteEndpoints();

app.Run();