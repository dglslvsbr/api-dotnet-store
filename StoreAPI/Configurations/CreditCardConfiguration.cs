using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreAPI.Entities.Models;

namespace StoreAPI.Configurations;

public class CreditCardConfiguration : IEntityTypeConfiguration<CreditCard>
{
    public void Configure(EntityTypeBuilder<CreditCard> builder)
    {
        // Primary Key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Number).IsRequired().HasMaxLength(16);
        builder.Property(x => x.Expiration).IsRequired();
        builder.Property(x => x.CVV).IsRequired().HasMaxLength(3);
        builder.Property(x => x.UsedLimit).HasDefaultValue(0.0m).HasPrecision(10, 2);
        builder.Property(x => x.MaxLimit).IsRequired().HasPrecision(10, 2);
        builder.Property(x => x.ClientId).IsRequired();

        // Unique
        builder.HasIndex(x => x.Number).IsUnique();
    }
}