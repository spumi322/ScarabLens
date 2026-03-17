namespace ScarabLens;

public interface IPriceCache
{
    decimal? GetPrice(string name);
}
