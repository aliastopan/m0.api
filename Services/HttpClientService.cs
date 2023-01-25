using Microsoft.Extensions.Options;
using System.Net.Http;

namespace m0.api.Services;

public class HttpClientService : BackgroundService
{
    private readonly Settings _settings;
    private readonly PeriodicTimer _timer;
    private readonly HttpClient _httpClient;

    public HttpClientService(IOptions<Settings> settings)
    {
        _settings = settings.Value;
        _timer = new PeriodicTimer(_settings.Delay);
        _httpClient = new HttpClient();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(await _timer.WaitForNextTickAsync(stoppingToken)
            && !stoppingToken.IsCancellationRequested)
        {
            await ClientAsync();
        }
    }

    private async Task ClientAsync()
    {
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri("https://official-joke-api.appspot.com/random_joke"),
            Method = HttpMethod.Get
        };
        request.Headers.Add("Accept", "*/*");
        request.Headers.Add("User-Agent", "Thunder Client (https://www.thunderclient.com)");

        var response = await _httpClient.SendAsync(request);
        var result = await response.Content.ReadAsStringAsync();
        await Task.CompletedTask;

        Log.Information("Result {0}", result);
    }

    private static async Task DoWorkAsync()
    {
        Log.Information(DateTime.Now.ToString("O"));
        await Task.CompletedTask;
    }
}
