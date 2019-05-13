using Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.EF
{
    public class ApplicationIdentityContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationIdentityContext(DbContextOptions<ApplicationIdentityContext> options)
            : base(options){}
    }
}
