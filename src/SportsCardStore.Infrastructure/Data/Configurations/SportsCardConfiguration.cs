using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsCardStore.Core.Entities;

namespace SportsCardStore.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for SportsCard entity
/// </summary>
public class SportsCardConfiguration : IEntityTypeConfiguration<SportsCard>
{
    public void Configure(EntityTypeBuilder<SportsCard> builder)
    {
        // Primary key
        builder.HasKey(sc => sc.Id);

        // Property configurations
        builder.Property(sc => sc.Id)
            .ValueGeneratedOnAdd();

        builder.Property(sc => sc.PlayerName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("TEXT");

        builder.Property(sc => sc.Year)
            .IsRequired()
            .HasColumnType("INTEGER");

        builder.Property(sc => sc.Brand)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("TEXT");

        builder.Property(sc => sc.CardNumber)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnType("TEXT");

        builder.Property(sc => sc.Sport)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(sc => sc.Team)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("TEXT");

        builder.Property(sc => sc.SetName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("TEXT");

        builder.Property(sc => sc.IsRookie)
            .IsRequired()
            .HasColumnType("INTEGER")
            .HasDefaultValue(false);

        builder.Property(sc => sc.IsAutograph)
            .IsRequired()
            .HasColumnType("INTEGER")
            .HasDefaultValue(false);

        builder.Property(sc => sc.IsRelic)
            .IsRequired()
            .HasColumnType("INTEGER")
            .HasDefaultValue(false);

        builder.Property(sc => sc.IsBowmanFirst)
            .IsRequired()
            .HasColumnType("INTEGER")
            .HasDefaultValue(false);

        builder.Property(sc => sc.Grade)
            .HasColumnType("REAL")
            .IsRequired(false);

        builder.Property(sc => sc.GradingCompany)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(sc => sc.Condition)
            .HasMaxLength(200)
            .HasColumnType("TEXT")
            .IsRequired(false);

        builder.Property(sc => sc.Price)
            .IsRequired()
            .HasColumnType("REAL");

        builder.Property(sc => sc.Quantity)
            .IsRequired()
            .HasColumnType("INTEGER");

        builder.Property(sc => sc.ImageUrl)
            .HasMaxLength(500)
            .HasColumnType("TEXT")
            .IsRequired(false);

        builder.Property(sc => sc.Description)
            .HasMaxLength(1000)
            .HasColumnType("TEXT")
            .IsRequired(false);

        builder.Property(sc => sc.IsAvailable)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(sc => sc.CreatedDate)
            .IsRequired()
            .HasColumnType("TEXT")
            .HasDefaultValueSql("datetime('now')");

        builder.Property(sc => sc.UpdatedDate)
            .IsRequired()
            .HasColumnType("TEXT")
            .HasDefaultValueSql("datetime('now')");

        // Indexes for performance
        builder.HasIndex(sc => sc.PlayerName)
            .HasDatabaseName("IX_SportsCards_PlayerName");

        builder.HasIndex(sc => sc.Year)
            .HasDatabaseName("IX_SportsCards_Year");

        builder.HasIndex(sc => new { sc.PlayerName, sc.Year })
            .HasDatabaseName("IX_SportsCards_PlayerName_Year");

        builder.HasIndex(sc => sc.Sport)
            .HasDatabaseName("IX_SportsCards_Sport");

        builder.HasIndex(sc => sc.IsAvailable)
            .HasDatabaseName("IX_SportsCards_IsAvailable");

        builder.HasIndex(sc => sc.CreatedDate)
            .HasDatabaseName("IX_SportsCards_CreatedDate");

        // Table configuration
        builder.ToTable("SportsCards");
    }
}