using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreAPI.Entities.Models;

namespace StoreAPI.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        // Primary Key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Quantity).IsRequired();
        builder.Property(x => x.UnitPrice).IsRequired().HasPrecision(10, 2);
        builder.Property(x => x.OrderId).IsRequired();
        builder.Property(x => x.ProductId).IsRequired();
    }
}