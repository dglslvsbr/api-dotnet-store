using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreAPI.Entities.Models;

namespace StoreAPI.Configurations
{
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
            builder.HasData(
                new Product
                {
                    Id = 1,
                    Name = "Graphic Card 4070 Ti",
                    Description = "It is a video card with excellent cost-effectiveness, extremely efficient and capable of delivering better performance in games.",
                    ImageUrl = "Utils/Images/GraphicCard4070Ti.png",
                    Price = 7.500m,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 2,
                    Name = "Graphic Card 5090",
                    Description = "It is a video card with excellent cost-effectiveness, extremely efficient and capable of delivering better performance in games.",
                    ImageUrl = "Utils/Images/GraphicCard5090.png",
                    Price = 22.999m,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 3,
                    Name = "Console Sony PlayStation 5",
                    Description = "The latest console from Sony, more efficient than previous generations, delivering maximum performance and excellent graphics.",
                    ImageUrl = "Utils/Image/ConsolePs5.png",
                    Price = 3.299m,
                    CategoryId = 2
                },
                new Product
                {
                    Id = 4,
                    Name = "Console Sony PlayStation 4",
                    Description = "Console from Sony, more efficient than previous generations, delivering maximum performance and excellent graphics.",
                    ImageUrl = "Utils/Image/ConsolePs4.png",
                    Price = 2.299m,
                    CategoryId = 2
                },
                new Product
                {
                    Id = 5,
                    Name = "Xiaomi Redmi Note 13",
                    Description = "It is an excellent cost-effective cell phone, currently considered the best of today, with a 150mpx camera.",
                    ImageUrl = "Utils/Image/XiaomiRedmiNote13.png",
                    Price = 3.599m,
                    CategoryId = 3
                },
                new Product
                {
                    Id = 6,
                    Name = "Iphone 15",
                    Description = "It is an excellent cost-effective cell phone, currently considered the best of today, with a 150mpx camera.",
                    ImageUrl = "Utils/Image/Iphone15.png",
                    Price = 6.500m,
                    CategoryId = 3
                });
        }
    }
}