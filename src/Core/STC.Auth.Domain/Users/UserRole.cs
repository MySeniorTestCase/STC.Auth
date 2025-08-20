using Microsoft.AspNetCore.Identity;
using STC.Auth.Domain.Roles;

namespace STC.Auth.Domain.Users;

public class UserRole : IdentityUserRole<string>
{
    public virtual User? User { get; set; }
    public virtual Role? Role { get; set; }
}