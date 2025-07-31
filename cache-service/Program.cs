using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("REDIS_CONNECTIONSTRING") ?? "localhost:6379";
    options.InstanceName = "EcommerceCache:";
});

var app = builder.Build();

app.MapGet("/produto/{id}", async (string id, IDistributedCache cache) =>
{
    var cachedData = await cache.GetStringAsync(id);
    if (cachedData is null)
        return Results.NotFound();

    var produto = JsonSerializer.Deserialize<Produto>(cachedData);
    return Results.Ok(produto);
});

app.MapPost("/produto", async (Produto produto, IDistributedCache cache) =>
{
    var json = JsonSerializer.Serialize(produto);
    await cache.SetStringAsync(produto.Id, json, new DistributedCacheEntryOptions
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
    });

    return Results.Ok(produto);
});

app.MapDelete("/produto/{id}", async (string id, IDistributedCache cache) =>
{
    await cache.RemoveAsync(id);
    return Results.NoContent();
});

app.Run();
record Produto (string Id, string Name, decimal Price);