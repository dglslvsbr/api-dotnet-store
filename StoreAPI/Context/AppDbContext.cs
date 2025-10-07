using Microsoft.EntityFrameworkCore;
using StoreAPI.Entities.Authentication;
using StoreAPI.Entities.Models;

namespace StoreAPI.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Client> Client { get; set; }
    public DbSet<Address> Address { get; set; }
    public DbSet<CreditCard> CreditCard { get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<OrderItem> OrderItem { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<ClientRole> ClientRole { get; set; }
    public DbSet<Role> Role { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}