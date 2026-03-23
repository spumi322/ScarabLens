using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ScarabLens;

public sealed class OverlayViewModel : INotifyPropertyChanged
{
    private readonly IPriceCache _cache;

    private string _statusText = "ScarabLens Active";

    public OverlayViewModel(IPriceCache cache)
    {
        _cache = cache;
    }

    public string StatusText
    {
        get => _statusText;
        set { _statusText = value; OnPropertyChanged(); }
    }

    /// <summary>
    /// Fired on the calling thread with a (formatted text, raw price) pair per slot
    /// (index-matched to <see cref="SlotPositions.All"/>). Price is null when not found.
    /// Subscriber is responsible for dispatching to the UI thread.
    /// </summary>
    public event Action<IReadOnlyList<(string Text, decimal? Price)>>? PricesRefreshed;

    /// <summary>Reads all slot prices from the cache and raises <see cref="PricesRefreshed"/>.</summary>
    public void RefreshDisplay()
    {
        var prices = SlotPositions.All
            .Select(slot =>
            {
                var price = _cache.GetPrice(slot.Name);
                return (Text: price is decimal p ? FormatPrice(p) : "?", Price: price);
            })
            .ToList();

        PricesRefreshed?.Invoke(prices);
    }

    private static string FormatPrice(decimal price)
    {
        var s = price.ToString("0.#");
        if (s.Length > 3) s = ((int)price).ToString();
        return s;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
