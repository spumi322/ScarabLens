using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;

namespace ScarabLens;

public sealed class ApiService : IApiService
{
    private static readonly HttpClient _http = CreateHttpClient();

    private static readonly JsonSerializerOptions _jsonOptions =
        new() { PropertyNameCaseInsensitive = true };

    private const string Url =
        "https://poe.ninja/poe1/api/economy/exchange/current/overview?league=Mirage&type=Scarab&language=en";

    private static HttpClient CreateHttpClient()
    {
        var client = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
        client.DefaultRequestHeaders.UserAgent.ParseAdd("ScarabLens/1.0");
        return client;
    }

    public async Task<Dictionary<string, decimal>> FetchScarabsAsync()
    {
        try
        {
            var json     = await _http.GetStringAsync(Url);
            var response = JsonSerializer.Deserialize<ScarabResponse>(json, _jsonOptions)
                           ?? new ScarabResponse();

            return response.Lines
                .Join(response.Items,
                    line => line.Id,
                    item => item.Id,
                    (line, item) => (item.Name, line.PrimaryValue))
                .GroupBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First().PrimaryValue, StringComparer.OrdinalIgnoreCase);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[ApiService] FetchScarabsAsync failed: {ex.Message}");
            return [];
        }
    }
}
