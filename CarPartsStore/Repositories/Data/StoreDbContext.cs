using Microsoft.EntityFrameworkCore;
using CarPartsStore.Models;

namespace CarPartsStore.Repositories.Data
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options)
            :base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ContactUsModel> Contacts { get; set; }
        public DbSet<HeadSetting> HeadSettings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TimelineModel> TimelineObjects { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<AdminUser> Admins { get; set; }
        public DbSet<AdminCandidate> Candidates { get; set; }
    }
}
