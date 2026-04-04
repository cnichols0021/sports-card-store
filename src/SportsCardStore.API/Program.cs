using Microsoft.EntityFrameworkCore;
using SportsCardStore.Core.Interfaces;
using SportsCardStore.Infrastructure.Data;
using SportsCardStore.Infrastructure.Data.Seeders;
using SportsCardStore.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Entity Framework
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

// Add Blob Storage Service
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
