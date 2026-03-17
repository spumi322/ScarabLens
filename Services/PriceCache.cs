using System.Diagnostics;

namespace ScarabLens;

public sealed class PriceCache : IPriceCache
{
    private readonly IApiService _api;

    // Replaced atomically on each refresh — safe to read from any thread on win-x64
    private Dictionary<string, decimal> _prices = new(StringComparer.OrdinalIgnoreCase);

    public DateTime LastFetched { get; private set; }

    public PriceCache(IApiService api)
    {
        _api = api;
        _ = Task.Run(RunPeriodicRefreshAsync);
    }

    public async Task RefreshAsync()
    {
        var dict    = await _api.FetchScarabsAsync();
        _prices     = dict;
        LastFetched = DateTime.UtcNow;
    }

    public decimal? GetPrice(string name)
        => _prices.TryGetValue(name, out var v) ? v : null;

    private async Task RunPeriodicRefreshAsync()
    {
        using var timer = new PeriodicTimer(TimeSpan.FromHours(1));
        while (await timer.WaitForNextTickAsync())
        {
            try   { await RefreshAsync(); }
            catch (Exception ex) { Debug.WriteLine($"[PriceCache] Periodic refresh failed: {ex.Message}"); }
        }
    }
}
