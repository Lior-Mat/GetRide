using System;
using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Services;

namespace SearchService.Data;

public class DbInitializer
{
    public static async Task InitDb(WebApplication app)
    {
        await DB.InitAsync("SearchDb", MongoClientSettings
            .FromConnectionString(app.Configuration.GetConnectionString("MongoDb")));

        await DB.Index<Item>()
            .Key(x => x.make, KeyType.Text)
            .Key(x => x.model, KeyType.Text)
            .Key(x => x.color, KeyType.Text)
            .CreateAsync();

        var counts = await DB.CountAsync<Item>();

        using var scope = app.Services.CreateAsyncScope();
        var httpClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();
        var items = await httpClient.GetItemsForSearchDb();
        Console.WriteLine(items.Count + "returned from the auction service");

        if (items.Count > 0) await DB.SaveAsync(items);
    }

}

