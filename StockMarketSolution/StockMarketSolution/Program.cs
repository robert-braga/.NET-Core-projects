using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;
using StockMarketSolution;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// To activate Options pattern for a section of configuration
builder.Services
    .Configure<TradingOptions>(
        builder.Configuration.GetSection("TradingOptions")
    );

builder.Services
    .AddSingleton<IFinnhubService, FinhubService>();
builder.Services
    .AddScoped<IStocksService, StocksService>();

builder.Services
    .AddSingleton<IFinnhubRepository, FinnhubRepository>();
builder.Services
    .AddScoped<IStocksRepository, StocksRepository>();

builder.Services.AddHttpClient();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("StockMarketDb"));
});

var app = builder.Build();

Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
