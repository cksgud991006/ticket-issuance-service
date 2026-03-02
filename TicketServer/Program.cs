using Microsoft.EntityFrameworkCore;
using TicketServer.Endpoints;
using TicketServer.Application.Services;
using TicketServer.Application.Repositories;
using TicketServer.Infrastructure.Database;
using StackExchange.Redis;
using TicketServer.Schedule;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// register DIs
var redisConnectionString = new ConfigurationOptions
{
    EndPoints = { "127.0.0.1:6379" },
    AbortOnConnectFail = false, // Crucial: lets the app start even if Redis is slow to respond
    ConnectTimeout = 5000,
    SyncTimeout = 5000
};
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));
builder.Services.AddScoped<IJobScheduler, JobScheduler> ();
builder.Services.AddSingleton<IJobRunner, JobRunner> ();
builder.Services.AddScoped<IQueueingService, QueueingService> ();
builder.Services.AddScoped<ISessionService, SessionService> ();
builder.Services.AddScoped<ISeatInventoryService, SeatInventoryService> ();
builder.Services.AddScoped<ISeatInventoryRepository, SeatInventoryRepository> ();
builder.Services.AddHostedService<TaskRunnerService>();
builder.Services.AddHostedService<DbInitializer>();
builder.Services.AddHostedService<SeatInventoryLoader>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
     throw new InvalidOperationException("Connection string 'AppDbContext'" +
    " not found.");

builder.Services.AddDbContext<SeatContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

app.UseCors();

app.MapTicketEndPoints();

app.Run();
