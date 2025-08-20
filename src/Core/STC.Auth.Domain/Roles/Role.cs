using Microsoft.AspNetCore.Identity;
using STC.Auth.Domain.Users;

namespace STC.Auth.Domain.Roles;

public class Role : IdentityRole
{
    public virtual ICollection<UserRole> UserRoles { get; set; } = [];
}