using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Database;
using UrlShortener.Services;

var builder = WebApplication.CreateBuilder(args);

// Register the database context
builder.Services.AddDbContext<UrlShortenerDbContext>(options => options.UseSqlite("Data Source=urls.db"));

builder.Services.AddScoped<IUrlShortenerService, UrlShortenerService>();

var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UrlShortenerDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.MapPost("/shorten", async ([FromBody] ShortenRequest request, HttpContext context, IUrlShortenerService urlShortenerService) =>
{
    if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
    {
        return Results.BadRequest("Invalid URL");
    }

    var shortCode = await urlShortenerService.ShortenAndStoreUrl(request.Url);
    var response = new { ShortenedUrl = $"{context.Request.Scheme}://{context.Request.Host}/{shortCode}" };

    return Results.Json(response);
});

app.MapGet("/{code}", async (string code, IUrlShortenerService urlShortenerService) =>
{
    var longUrl = await urlShortenerService.GetOriginalUrl(code);
    if (longUrl is null)
    {
        return Results.NotFound("URL not found");
    }

    return Results.Redirect(longUrl);
});

app.Run();

// Model for shortening request
internal record ShortenRequest(string Url);