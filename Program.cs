using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.Data;
using WebApp.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options=>{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:ProductConnection"]);
    options.EnableSensitiveDataLogging(true);
});


var app = builder.Build();

app.UseMiddleware<TestMiddleware>();

app.MapGet("/", () => "Hello World!");


var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
SeedData.SeedDatabase(context);

app.Run();
