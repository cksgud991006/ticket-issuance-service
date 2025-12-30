using Microsoft.EntityFrameworkCore;
using CouponServer.Endpoints;
using CouponServer.Services;
using CouponServer.Repositories;

var builder = WebApplication.CreateBuilder(args);

// register DIs
builder.Services.AddScoped<ICouponService, CouponIssueService> ();
builder.Services.AddScoped<ICouponRepository, CouponRepository> ();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
     throw new InvalidOperationException("Connection string 'AppDbContext'" +
    " not found.");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

DbInitializer.Seed(app.Services);

app.MapCouponEndPoints();

app.Run();
