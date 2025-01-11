using LicenseManagerCloud.Models;
using Microsoft.EntityFrameworkCore;

namespace LicenseManagerCloud.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Lincense> Licenses { get; set; }
    }
}
