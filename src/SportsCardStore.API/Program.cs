using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using SportsCardStore.Core.Interfaces;
using SportsCardStore.Infrastructure.Data;
using SportsCardStore.Infrastructure.Data.Seeders;
using SportsCardStore.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Auto-detect database provider from the connection string:
//   SQLite  : "Data Source=*.db"  — local development, no migrations needed
//   SqlServer: anything else       — Azure / production, uses EF migrations
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var isSqlite = connectionString != null &&
               connectionString.StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase) &&
               connectionString.EndsWith(".db", StringComparison.OrdinalIgnoreCase);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (isSqlite)
        options.UseSqlite(connectionString);
    else
        options.UseSqlServer(connectionString);
});

// Add Blob Storage Service
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

// Add Sports Card Service
builder.Services.AddScoped<ISportsCardService, SportsCardService>();

// Configure multipart form data options for file uploads
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10MB
});

builder.Services.AddControllers();

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        var origins = new List<string>
        {
            "http://localhost:3000",  // CRA default
            "http://localhost:3001",  // CRA / Vite fallback
            "http://localhost:3002",  // CRA / Vite fallback
            "http://localhost:5173",  // Vite default
            "http://localhost:5174"   // Vite fallback
        };
        
        // Add production frontend URL if configured
        var frontendUrl = builder.Configuration["FrontendUrl"];
        if (!string.IsNullOrEmpty(frontendUrl))
        {
            origins.Add(frontendUrl);
        }
        
        policy.WithOrigins(origins.ToArray())
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Initialize database on startup
using var scope = app.Services.CreateScope();
try
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (isSqlite)
    {
        // SQLite local dev: EnsureCreated builds the schema directly from the
        // current model — no migrations required, no provider-compatibility issues.
        await context.Database.EnsureCreatedAsync();
    }
    else
    {
        // SQL Server (Azure): apply any pending EF migrations.
        await context.Database.MigrateAsync();
    }

    // Only seed data in development
    if (app.Environment.IsDevelopment())
    {
        await SportsCardSeeder.SeedAsync(context);
    }
}
catch (Exception ex)
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while initializing the database.");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
