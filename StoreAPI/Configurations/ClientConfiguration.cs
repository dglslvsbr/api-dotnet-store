using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreAPI.Entities.Models;

namespace StoreAPI.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        // Primary Key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(30);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(30);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(30);
        builder.Property(x => x.Password).IsRequired().HasMaxLength(100);
        builder.Property(x => x.CPF).IsRequired().HasMaxLength(11);
        builder.Property(x => x.Phone).IsRequired().HasMaxLength(11);

        // Unique
        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasIndex(x => x.Phone).IsUnique();
        builder.HasIndex(x => x.CPF).IsUnique();

        // Relations
        builder.HasOne(x => x.Address).WithOne(x => x.Client);
        builder.HasOne(x => x.CreditCard).WithOne(x => x.Client);
        builder.HasMany(x => x.Order).WithOne(x => x.Client);
        builder.HasMany(x => x.ClientRole).WithOne(x => x.Client);
    }
}