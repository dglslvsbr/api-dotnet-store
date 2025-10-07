using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreAPI.Entities.Authentication;

namespace StoreAPI.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Primary Key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Name).IsRequired().HasMaxLength(30);

        // Relations
        builder.HasMany(x => x.ClientRole).WithOne(x => x.Role);
    }
}