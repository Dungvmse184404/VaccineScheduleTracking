using Microsoft.EntityFrameworkCore;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Data
{
    public class VaccineScheduleDbContext : DbContext
    {
        public VaccineScheduleDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) 
        {
            
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Child> Children { get; set; } 
    }
}
