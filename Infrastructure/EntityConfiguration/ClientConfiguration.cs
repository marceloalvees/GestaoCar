using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfiguration
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Clients");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.CPF)
                .IsRequired()
                .HasMaxLength(11);
            builder.HasIndex(c => c.CPF)
                .IsUnique();

            builder.Property(c => c.Phone)
                .IsRequired()
                .HasMaxLength(15);
            builder.HasMany(c => c.Sales)
                .WithOne(s => s.Client)
                .HasForeignKey(s => s.ClientId);
        }
    }
}
