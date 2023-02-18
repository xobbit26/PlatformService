using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlatformService.Data.Entities;

namespace PlatformService.Data.EntityConfigurations;

public class PlatformEntityTypeConfiguration : IEntityTypeConfiguration<Platform>
{
    public void Configure(EntityTypeBuilder<Platform> builder)
    {
        builder.Property(p => p.Name)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(p => p.Publisher)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(p => p.Cost)
            .HasMaxLength(100)
            .IsRequired();
    }
}