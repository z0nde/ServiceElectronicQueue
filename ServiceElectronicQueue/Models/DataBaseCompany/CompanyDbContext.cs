using Microsoft.EntityFrameworkCore;

namespace ServiceElectronicQueue.Models.DataBaseCompany
{
    public class CompanyDbContext : DbContext
    {
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<BranchOffice> BranchOffices { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<ServiceSector> Services { get; set; }
        public virtual DbSet<ElectronicQueue> ElectronicQueues { get; set; }

        /*public CompanyDbContext()
        { }*/
        
        public CompanyDbContext(DbContextOptions<CompanyDbContext> dbContextOptions) : base(dbContextOptions)
        {
            //Database.EnsureCreated();
            //AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
    }
}