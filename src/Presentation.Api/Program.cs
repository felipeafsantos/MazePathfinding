using System.Reflection;
using Application.Service;
using Application.Service.PathfindingStrategies;
using Data.Repository;
using Domain.Model;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddScoped<IMazeStrategy,HeroStrategy>()
    .AddScoped<IMazeService, MazeService>();

builder.Services
    .AddSingleton<IRepository<Maze>, MazeRepository>();

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Parcel Hero - MazePathfinding API",
        Description = "An ASP.NET Web API for resolving mazes"
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
