using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreAPI.Entities.Models;

namespace StoreAPI.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        // Primary Key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.CreatAt).IsRequired();
        builder.Property(x => x.CurrentState).IsRequired();
        builder.Property(x => x.Installments).IsRequired();
        builder.Property(x => x.ClientId).IsRequired();

        // Ignore
        builder.Ignore(x => x.OrderTotal);

        // Relations
        builder.HasMany(x => x.OrderItem).WithOne(x => x.Order).HasForeignKey(x => x.OrderId);
    }
}