using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreAPI.Entities.Models;
using StoreAPI.Useful;

namespace StoreAPI.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Primary Key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Name).IsRequired().HasMaxLength(30);
        builder.Property(x => x.Price).IsRequired().HasPrecision(10, 2);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(500);
        builder.Property(x => x.ImageUrl).IsRequired().HasMaxLength(100);
        builder.Property(x => x.CategoryId).IsRequired();

        // Relations
        builder.HasMany<OrderItem>().WithOne(x => x.Product);

        // Data Seeding
        builder.HasData(ProductHasData.Products());
    }
}