namespace UrlShortener.Services;

public interface IUrlShortenerService
{
    string ShortenAndStoreUrl(string longUrl);
    string? GetOriginalUrl(string shortCode);
}