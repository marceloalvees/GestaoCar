using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfiguration
{
    public class DealershipConfiguration : IEntityTypeConfiguration<Dealership>
    {
        public void Configure(EntityTypeBuilder<Dealership> builder)
        {
            builder.ToTable("Dealerships");
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasIndex(d => d.Name)
                .IsUnique();

            builder.Property(d => d.Address)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(d => d.City)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(d => d.State)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(d => d.ZipCode)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(d => d.Phone)
                .IsRequired()
                .HasMaxLength(15);
            builder.Property(d => d.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.MaxVehicleCapacity)
                .IsRequired();

            builder.HasMany(d => d.Sales)
                .WithOne(s => s.Dealership)
                .HasForeignKey(s => s.DealershipId);
        }
    }
}
