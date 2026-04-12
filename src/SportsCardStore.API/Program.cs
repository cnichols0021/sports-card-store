using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using SportsCardStore.Core.Interfaces;
using SportsCardStore.Infrastructure.Data;
using SportsCardStore.Infrastructure.Data.Seeders;
using SportsCardStore.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Entity Framework
// Auto-detect provider from the connection string:
//   - SQLite  : connection string starts with "Data Source=" and ends with ".db" (local dev)
//   - SqlServer: everything else (Azure / production)
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    if (connectionString != null &&
        connectionString.StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase) &&
        connectionString.EndsWith(".db", StringComparison.OrdinalIgnoreCase))
    {
        options.UseSqlite(connectionString);
    }
    else
    {
        options.UseSqlServer(connectionString);
    }
});

// Add Blob Storage Service
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

// Add Sports Card Service
builder.Services.AddScoped<ISportsCardService, SportsCardService>();

// Configure multipart form data options for file uploads
builder.Services.Configure<FormOptions>(options =>
{
    // Set maximum file size to 10MB
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10MB
});

builder.Services.AddControllers();

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",  // Vite dev server (default)
                "http://localhost:5174",  // Vite fallback port
                "http://localhost:3000"   // CRA / alternative dev servers
              )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed database in development environment
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Apply pending migrations
            await context.Database.MigrateAsync();

            // Seed data
            await SportsCardSeeder.SeedAsync(context);
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }
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
