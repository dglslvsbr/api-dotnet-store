using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreAPI.Entities.Authentication;

namespace StoreAPI.Configurations
{
    public class ClientRoleConfiguration : IEntityTypeConfiguration<ClientRole>
    {
        public void Configure(EntityTypeBuilder<ClientRole> builder)
        {
            // Primary Key & Foreign Key
            builder.HasKey(x => new { x.ClientId, x.RoleId });

            // Properties
            builder.Property(x => x.ClientId).IsRequired();
            builder.Property(x => x.RoleId).IsRequired();
        }
    }
}