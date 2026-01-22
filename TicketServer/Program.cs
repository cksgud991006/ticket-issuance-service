using Microsoft.EntityFrameworkCore;
using TicketServer.Endpoints;
using TicketServer.Services;
using TicketServer.Repositories;

var builder = WebApplication.CreateBuilder(args);

// register DIs
builder.Services.AddScoped<ITicketService, TicketIssueService> ();
builder.Services.AddScoped<ITicketRepository, TicketRepository> ();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
     throw new InvalidOperationException("Connection string 'AppDbContext'" +
    " not found.");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

DbInitializer.Seed(app.Services);

app.MapTicketEndPoints();

app.Run();
