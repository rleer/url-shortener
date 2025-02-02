namespace UrlShortener.Services;

public interface IUrlShortenerService
{
    Task<string> ShortenAndStoreUrl(string longUrl);
    Task<string?> GetOriginalUrl(string shortCode);
}