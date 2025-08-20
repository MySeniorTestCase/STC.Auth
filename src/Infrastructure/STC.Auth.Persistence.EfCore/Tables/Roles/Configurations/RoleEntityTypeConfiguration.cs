using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using STC.Auth.Domain.Roles;

namespace STC.Auth.Persistence.EfCore.Tables.Roles.Configurations;

public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(_role => _role.Id);

        builder.Property(_role => _role.Id).IsRequired().HasColumnOrder(0);
        builder.Property(_role => _role.Name).IsRequired().HasColumnOrder(1);
        builder.Property(_role => _role.NormalizedName).IsRequired().HasColumnOrder(2);
        builder.Property(_role => _role.ConcurrencyStamp).IsRequired().HasColumnOrder(3);
    }
}