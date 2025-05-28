using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Pincred.Template.Data.Configuration;
using Pincred.Template.Domain.Entities;

namespace Pincred.Template.Data.Contexts;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    internal DbSet<Summary> Summaries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SummaryConfiguration());

        SetDefaultLenght(modelBuilder);
    }   

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // On Model create
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

    private static void SetDefaultLenght(ModelBuilder modelBuilder)
    {
        // Apply global string length config after all specific configurations
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.IsOwned())
            {
                continue;
            }

            foreach (var property in entityType.GetProperties()
                .Where(p => p.ClrType == typeof(string) &&
                            p.GetMaxLength() == null &&
                            !IsTextColumn(p)))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property(property.Name)
                    .HasMaxLength(100);
            }
        }
    }

    private static bool IsTextColumn(IMutableProperty property)
    {
        var columnType = property.GetColumnType();
        return !string.IsNullOrEmpty(columnType) && columnType.Equals("text", StringComparison.OrdinalIgnoreCase);
    }
}