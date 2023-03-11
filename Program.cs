using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.Data;
using WebApp.Middlewares;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:ProductConnection"]);
    options.EnableSensitiveDataLogging(true);
});

// builder.Services


var app = builder.Build();

const string baseUrl = "api/products";

app.MapGet($"{baseUrl}/{{id}}", async (HttpContext context, DataContext data) =>
{
    string? id = context.Request.RouteValues["id"] as string;

    if (id != null)
    {
        Product? p = data.Products.Find(long.Parse(id));

        if (p != null)
        {
            context.Response.StatusCode = StatusCodes.Status200OK;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize<Product>(p));
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
        }
    }
});

app.MapGet(baseUrl, async (HttpContext context, DataContext data) =>
{
    context.Response.ContentType = "application/json";
    await context.Response.WriteAsync(JsonSerializer.Serialize<IEnumerable<Product>>(data.Products));
});

app.MapPost(baseUrl, async (HttpContext context, DataContext data) =>
{
    Product? p = await JsonSerializer.DeserializeAsync<Product>(context.Request.Body);

    if (p != null)
    {
        await data.AddAsync(p);
        await data.SaveChangesAsync();

        context.Response.StatusCode = StatusCodes.Status200OK;
    }
});


app.UseMiddleware<TestMiddleware>();

app.MapGet("/", () => "Hello World!");


var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
SeedData.SeedDatabase(context);

app.Run();
