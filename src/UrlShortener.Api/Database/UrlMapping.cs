using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Database;

public class UrlMapping
{
    [Key]
    public long Id { get; set; }
    public string ShortCode { get; set; } = string.Empty;
    public string LongUrl { get; set; } = string.Empty;
}