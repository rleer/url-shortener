using Microsoft.EntityFrameworkCore;
using UrlShortener.Database;

namespace UrlShortener.Services;

public class UrlShortenerService : IUrlShortenerService
{
    private static readonly char[] Chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    private readonly UrlShortenerDbContext _dbContext;

    public UrlShortenerService(UrlShortenerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private static string GenerateShortCode()
    {
        // Very naive implementation, we can improve this later
        return string.Create(5, Chars, (shortCode, charsState) => Random.Shared.GetItems(charsState, shortCode));
    }

    public async Task<string> ShortenAndStoreUrl(string longUrl)
    {
        // Check if the long URL is already in the dictionary
        var existing = _dbContext.UrlMappings.FirstOrDefault(u => u.LongUrl == longUrl);
        if (existing is not null)
        {
            return existing.ShortCode;
        }

        // Generate a new short code
        string shortCode;
        do
        {
            shortCode = GenerateShortCode();
        } 
        while (_dbContext.UrlMappings.Any(u => u.ShortCode == shortCode));
        
        var urlMapping = new UrlMapping
        {
            ShortCode = shortCode,
            LongUrl = longUrl
        };

        _dbContext.UrlMappings.Add(urlMapping);
        await _dbContext.SaveChangesAsync();

        return shortCode;
    }

    public async Task<string?> GetOriginalUrl(string shortCode)
    {
        var urlMapping = await _dbContext.UrlMappings.FirstOrDefaultAsync(u => u.ShortCode == shortCode);
        return urlMapping?.LongUrl;
    }
}