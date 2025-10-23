using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StoreAPI.Context;
using StoreAPI.Entities.Models;

namespace StoreAPI_Tests_Integration;

public class CustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                x => x.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (descriptor is not null)
                services.Remove(descriptor);

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("StoreDBTest");
            });

            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            db.Database.EnsureCreated();

            db.Client.RemoveRange(db.Client);
            db.Product.RemoveRange(db.Product);
            db.SaveChanges();

            db.Client.Add(new Client
            {
                Id = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "email@example.com",
                Password = "Password",
                CPF = "12345678911",
                Phone = "12345678911",
                Address = new()
                {
                    Id = 1,
                    Street = "Desconhecido",
                    Number = "Desconhecido",
                    District = "Desconhecido",
                    City = "Desconhecido",
                    State = "Desconhecido"
                },
                CreditCard = new()
                {
                    Id = 1,
                    Number = "1234567891234567",
                    Expiration = DateTimeOffset.Now.AddYears(5),
                    CVV = "123",
                    UsedLimit = 0m,
                    MaxLimit = 500m
                }
            });

            db.Product.AddRange(
                new Product { Id = 1, Name = "Computer", Description = "", Price = 50m, Quantity = 5, ImageUrl = "" },
                new Product { Id = 2, Name = "Teclado", Description = "", Price = 50m, Quantity = 5, ImageUrl = "" },
                new Product { Id = 3, Name = "Mouse", Description = "", Price = 50m, Quantity = 5, ImageUrl = "" }
            );

            db.SaveChanges();
        });
    }
}