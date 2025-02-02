using System.Collections.Concurrent;

namespace UrlShortener.Services;

public class UrlShortenerService : IUrlShortenerService
{
    private static readonly char[] Chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    private readonly ConcurrentDictionary<string, string> _urlStore = [];

    private static string GenerateShortCode()
    {
        // Very naive implementation, we can improve this later
        return string.Create(5, Chars, (shortCode, charsState) => Random.Shared.GetItems(charsState, shortCode));
    }

    public string ShortenAndStoreUrl(string longUrl)
    {
        // Check if the long URL is already in the dictionary
        var shortCode = _urlStore.FirstOrDefault(kv => kv.Value == longUrl).Key;
        if (!string.IsNullOrWhiteSpace(shortCode))
        {
            return shortCode;
        }
        
        // Generate a new short code
        while (true)
        {
            shortCode = GenerateShortCode();
            if (_urlStore.TryAdd(shortCode, longUrl))
            {
                return shortCode;
            }
        }
    }

    public string? GetOriginalUrl(string shortCode)
    {
        return _urlStore.GetValueOrDefault(shortCode);
    }
}