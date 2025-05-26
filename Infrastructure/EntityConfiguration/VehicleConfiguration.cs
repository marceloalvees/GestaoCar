using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfiguration
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.ToTable("Vehicles");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Model)
                 .HasMaxLength(100)
                 .IsRequired();

            builder.Property(v => v.ManuFacturingYear)
                .IsRequired();

            builder.Property(v => v.Price)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(v => v.VehicleType)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(v => v.Description)
                .HasColumnType("text")
                .IsRequired(false);

            builder.Property<int>("ManufacturerId")
                .HasColumnName("ManufacturerId")
                .IsRequired();

            builder.HasOne(v => v.Manufacturer)
                .WithMany(m => m.Vehicles)
                .HasForeignKey("ManufacturerId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(v => v.Sales)
                .WithOne(s => s.Vehicle)
                .HasForeignKey(s => s.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
