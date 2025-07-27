using System;
using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;

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

        if (counts == 0)
        {
            System.Console.WriteLine("No Data - will attempt to seed");
            var itemData = await File.ReadAllTextAsync("Data/auctions.json");
            //Console.WriteLine(itemData);

            var options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true };
            var items = JsonSerializer.Deserialize<List<Item>>(itemData, options);

            await DB.SaveAsync(items);
        }
    }

}

