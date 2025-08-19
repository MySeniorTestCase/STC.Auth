using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using STC.Domain.Roles;
using STC.Domain.Users;

namespace STC.Persistence.EfCore;

public class AuthDbContext : IdentityDbContext<User, Role, string>
{
}