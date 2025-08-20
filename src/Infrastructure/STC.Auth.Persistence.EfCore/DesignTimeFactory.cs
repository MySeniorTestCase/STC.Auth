using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace STC.Auth.Persistence.EfCore;

public class DesignTimeFactory : IDesignTimeDbContextFactory<AuthDbContext>
{
    public AuthDbContext CreateDbContext(string[] args)
    {
        string connectionString = args[0] ??
                                  throw new ArgumentNullException(message: "Connection string is required",
                                      innerException: null);


        var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();
        optionsBuilder.UseNpgsql(connectionString: connectionString);

        return new AuthDbContext(optionsBuilder.Options);
    }
}