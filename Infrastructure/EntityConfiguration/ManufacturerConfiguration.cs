using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfiguration
{
    public class ManufacturerConfiguration : IEntityTypeConfiguration<Manufacturer>
    {
        public void Configure(EntityTypeBuilder<Manufacturer> builder)
        {
            builder.ToTable("Manufacturers");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(m => m.Name)
                .IsUnique();

            builder.Property(m => m.Country)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.FoundationYear)
                .IsRequired();

            builder.Property(m => m.Website)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasMany(m => m.Vehicles)
               .WithOne(v => v.Manufacturer)
               .HasForeignKey(v => v.Id);
        }
    }
}
