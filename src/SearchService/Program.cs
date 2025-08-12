using Microsoft.VisualBasic;
using MongoDB.Driver;
using MongoDB.Entities;
using Polly;
using Polly.Extensions.Http;
using SearchService;
using SearchService.Data;
using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddHttpClient<AuctionSvcHttpClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:7001"); 
}).AddPolicyHandler(GetPolicy());



var app = builder.Build();


app.UseAuthorization();

app.MapControllers();

try
{
    await DbInitializer.InitDb(app);
}
catch (Exception e)
{
    System.Console.WriteLine(e);
}

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetPolicy()

    => HttpPolicyExtensions
    .HandleTransientHttpError()
    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
    .RetryForeverAsync(_ => TimeSpan.FromMicroseconds(3));

