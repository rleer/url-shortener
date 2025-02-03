using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Database;

public class UrlShortenerDbContext : DbContext
{
    public DbSet<UrlMapping> UrlMappings { get; set; }
    
    public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options) : base(options)
    {
    }
}