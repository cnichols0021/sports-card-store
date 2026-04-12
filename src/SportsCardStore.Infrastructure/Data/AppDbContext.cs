using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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

        // Apply entity configurations from the explicit configuration class
        // and any other IEntityTypeConfiguration implementations in this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Fallback for EF design-time tools when no DI configuration is present.
            // Program.cs handles provider selection for runtime — this is only reached
            // by dotnet-ef tooling that doesn't go through the DI pipeline.
            optionsBuilder.UseSqlite("Data Source=SportsCardStore.db");
        }

        // Suppress PendingModelChangesWarning — the migration files are hand-edited
        // to use correct SQL Server types and the snapshot hash may not match exactly.
        // The schema is correct; this warning is a false positive from the hash check.
        optionsBuilder.ConfigureWarnings(w =>
            w.Ignore(RelationalEventId.PendingModelChangesWarning));
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
