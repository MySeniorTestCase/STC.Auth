using Microsoft.AspNetCore.Identity;
using STC.Domain.Users;

namespace STC.Domain.Roles;

public class Role : IdentityRole
{
    public virtual ICollection<UserRole> UserRoles { get; set; } = [];
}