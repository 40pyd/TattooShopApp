using Identity.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Identity.Services
{
    public class ApplicationIdentityContextFactory : IDesignTimeDbContextFactory<ApplicationIdentityContext>
    {
        public ApplicationIdentityContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationIdentityContext>();
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            optionsBuilder.UseSqlServer(configBuilder.GetConnectionString("DefaultConnection"));
            return new ApplicationIdentityContext(optionsBuilder.Options);
        }
    }
}
