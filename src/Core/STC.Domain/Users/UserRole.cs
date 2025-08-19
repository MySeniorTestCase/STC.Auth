using Microsoft.AspNetCore.Identity;
using STC.Domain.Roles;

namespace STC.Domain.Users;

public class UserRole : IdentityUserRole<string>
{
    public virtual User? User { get; set; }
    public virtual Role? Role { get; set; }
}