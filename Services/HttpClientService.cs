using Microsoft.Extensions.Options;

namespace m0.api.Services;

public class HttpClientService : BackgroundService
{
    private readonly Settings _settings;
    private readonly PeriodicTimer _timer;

    public HttpClientService(IOptions<Settings> settings)
    {
        _settings = settings.Value;
        _timer = new PeriodicTimer(_settings.Delay);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            await DoWorkAsync();
        }
    }

    private static async Task DoWorkAsync()
    {
        Log.Information(DateTime.Now.ToString("O"));
        await Task.CompletedTask;
    }
}
