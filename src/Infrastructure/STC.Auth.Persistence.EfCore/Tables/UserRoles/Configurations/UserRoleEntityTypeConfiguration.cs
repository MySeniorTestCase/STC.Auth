using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using STC.Auth.Domain.Roles;
using STC.Auth.Domain.Users;

namespace STC.Auth.Persistence.EfCore.Tables.UserRoles.Configurations;

public class UserRoleEntityTypeConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        // builder.HasKey(_userRole => new { _userRole.UserId, _userRole.RoleId });

        builder.HasOne<User>(_userRole => _userRole.User)
            .WithMany(_user => _user.UserRoles)
            .HasForeignKey(_userRole => _userRole.UserId);

        builder.HasOne<Role>(_userRole => _userRole.Role)
            .WithMany(_role => _role.UserRoles)
            .HasForeignKey(_userRole => _userRole.RoleId);
    }
}