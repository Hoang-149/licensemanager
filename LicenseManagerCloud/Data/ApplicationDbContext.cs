using Microsoft.EntityFrameworkCore;

namespace LicenseManagerCloud.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<License> Licenses { get; set; }
    }
}
