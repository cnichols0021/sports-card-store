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
            .HasColumnType("nvarchar(100)");

        builder.Property(sc => sc.Year)
            .IsRequired()
            .HasColumnType("int");

        builder.Property(sc => sc.Brand)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("nvarchar(50)");

        builder.Property(sc => sc.CardNumber)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnType("nvarchar(20)");

        builder.Property(sc => sc.Sport)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(sc => sc.Team)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("nvarchar(50)");

        builder.Property(sc => sc.SetName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("nvarchar(100)");

        builder.Property(sc => sc.IsRookie)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(false);

        builder.Property(sc => sc.IsAutograph)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(false);

        builder.Property(sc => sc.IsRelic)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(false);

        builder.Property(sc => sc.IsBowmanFirst)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(false);

        builder.Property(sc => sc.Grade)
            .HasColumnType("decimal(3,1)")
            .IsRequired(false);

        builder.Property(sc => sc.GradingCompany)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(sc => sc.Condition)
            .HasMaxLength(200)
            .HasColumnType("nvarchar(200)")
            .IsRequired(false);

        builder.Property(sc => sc.Price)
            .IsRequired()
            .HasColumnType("decimal(8,2)");

        builder.Property(sc => sc.Quantity)
            .IsRequired()
            .HasColumnType("int");

        builder.Property(sc => sc.ImageUrl)
            .HasMaxLength(500)
            .HasColumnType("nvarchar(500)")
            .IsRequired(false);

        builder.Property(sc => sc.Description)
            .HasMaxLength(1000)
            .HasColumnType("nvarchar(1000)")
            .IsRequired(false);

        builder.Property(sc => sc.IsAvailable)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(sc => sc.CreatedDate)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(sc => sc.UpdatedDate)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETUTCDATE()");

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