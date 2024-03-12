using Microsoft.EntityFrameworkCore;

namespace ServiceElectronicQueue.Models.DataBaseCompany
{
    public class CompanyDbContext : DbContext
    {
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<BranchOffice> BranchOffices { get; set; }
        public DbSet<User> Users { get; set; }

        public CompanyDbContext()
        { }
        
        public CompanyDbContext(DbContextOptions<CompanyDbContext> dbContextOptions) : base(dbContextOptions)
        {
            Database.EnsureCreated();
        }
    }
}