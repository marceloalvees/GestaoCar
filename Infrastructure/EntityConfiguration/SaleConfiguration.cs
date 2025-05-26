using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfiguration
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.VehicleId)
                .IsRequired();
            builder.Property(s => s.DealershipId)
                .IsRequired();
            builder.Property(s => s.ClientId)
                .IsRequired();
            builder.Property(s => s.SaleDate)
                .IsRequired();
            builder.Property(s => s.SalePrice)
                .IsRequired()
                .HasColumnType("decimal(10,2)");
            builder.Property(s => s.SaleProtocol)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasOne(s => s.Vehicle)
            .WithMany(v => v.Sales)
            .HasForeignKey(s => s.VehicleId);

            builder.HasOne(s => s.Dealership)
                .WithMany(d => d.Sales)
                .HasForeignKey(s => s.DealershipId);
        }
    }
}
