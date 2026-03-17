namespace ScarabLens;

public interface IApiService
{
    Task<Dictionary<string, decimal>> FetchScarabsAsync();
}
