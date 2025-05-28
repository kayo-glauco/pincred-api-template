using FHF.Template.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FHF.Template.Data.Configuration;

public class SummaryConfiguration : EntityBaseConfiguration<Summary>
{
    public override void Configure(EntityTypeBuilder<Summary> builder)
    {
        base.Configure(builder);

        builder.ToTable("tb_summaries");

        builder
            .Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(50);
    }
}