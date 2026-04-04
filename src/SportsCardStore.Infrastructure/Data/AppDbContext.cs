using Microsoft.EntityFrameworkCore;
using SportsCardStore.Core.Entities;
using SportsCardStore.Infrastructure.Data.Configurations;

namespace SportsCardStore.Infrastructure.Data;

/// <summary>
/// Entity Framework DbContext for the Sports Card Store application
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Sports cards collection
    /// </summary>
    public DbSet<SportsCard> SportsCards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply entity configurations
        modelBuilder.ApplyConfiguration(new SportsCardConfiguration());

        // Apply all configurations from current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // This will be overridden by dependency injection configuration
        // Only used for design-time tools if no other configuration is available
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer();
        }
    }

    /// <summary>
    /// Override SaveChanges to automatically update UpdatedDate
    /// </summary>
    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    /// <summary>
    /// Override SaveChangesAsync to automatically update UpdatedDate
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Automatically update CreatedDate and UpdatedDate for entities
    /// </summary>
    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries<SportsCard>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    entry.Entity.UpdatedDate = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedDate = DateTime.UtcNow;
                    // Prevent CreatedDate from being modified
                    entry.Property(e => e.CreatedDate).IsModified = false;
                    break;
            }
        }
    }
}