using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using STC.Auth.Domain.Users;

namespace STC.Auth.Persistence.EfCore.Tables.Users.Configurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(_user => _user.Id);

        builder.Property(_user => _user.Id).IsRequired().HasColumnOrder(0);
        builder.Property(_user => _user.UserName).IsRequired().HasColumnOrder(1).HasMaxLength(300);
        builder.Property(_user => _user.NormalizedUserName).IsRequired().HasColumnOrder(2).HasMaxLength(300);
        builder.Property(_user => _user.RefreshToken).IsRequired(false).HasColumnOrder(3).HasMaxLength(300);
        builder.Property(_user => _user.RefreshTokenExpiryDate).IsRequired(false).HasColumnOrder(4);
        builder.Property(_user => _user.SecurityStamp).IsRequired().HasColumnOrder(5).HasMaxLength(100);
    }
}