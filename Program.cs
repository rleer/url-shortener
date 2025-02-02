using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IUrlShortenerService, UrlShortenerService>();

var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();

app.MapPost("/shorten", ([FromBody] ShortenRequest request, HttpContext context, IUrlShortenerService urlShortenerService) =>
{
    if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
    {
        return Results.BadRequest("Invalid URL");
    }

    var shortCode = urlShortenerService.ShortenAndStoreUrl(request.Url);
    var response = new { ShortenedUrl = $"{context.Request.Scheme}://{context.Request.Host}/{shortCode}" };

    return Results.Json(response);
});

app.MapGet("/{code}", (string code, IUrlShortenerService urlShortenerService) =>
{
    var longUrl = urlShortenerService.GetOriginalUrl(code);
    if (longUrl is null)
    {
        return Results.NotFound("URL not found");
    }

    return Results.Redirect(longUrl);
});

app.Run();

// Model for shortening request
internal record ShortenRequest(string Url);