
using Dapper;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Repository_Pattern.Data;
using Repository_Pattern.Data.Handlers;
using Repository_Pattern.Endpoints;
using Repository_Pattern.Interfaces;
using Repository_Pattern.Repositories;
using System.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<IProductRepository, DapperProductRepository>();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source = app.db");
});

builder.Services.AddScoped<IDbConnection>(_ =>
    new SqliteConnection("Data Source=app.db"));

var app = builder.Build();

SqlMapper.AddTypeHandler(new GuidHandler());

app.MapProductEndpoints();

app.Run();