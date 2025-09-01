using BNPLiveCode.Api.Data;
using BNPLiveCode.Api.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

//Add DbContext with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register DI repositories
builder.Services.AddRepositories();
// Register DI services
builder.Services.AddServices();

// //register DI external services
builder.Services.AddSecuritiesDataProvider(builder.Configuration);


app.Run();

