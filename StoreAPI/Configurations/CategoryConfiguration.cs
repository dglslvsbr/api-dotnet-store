using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreAPI.Entities.Models;

namespace StoreAPI.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // Primary Key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Name).IsRequired();

        // Unique
        builder.HasIndex(x => x.Name).IsUnique();

        // Relations
        builder.HasMany(x => x.Product).WithOne(x => x.Category);

        // Data Seeding
        builder.HasData(
                    new Category { Id = 1, Name = "Hardware" },
                    new Category { Id = 2, Name = "PlayStation" },
                    new Category { Id = 3, Name = "Smartphone" });

    }
}