using Microsoft.EntityFrameworkCore;
using WebScoringApp.Models;
namespace WebScoringApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<GroupInformation> GroupInformations { get; set; }
        public DbSet<GroupItem> GroupItems { get; set; }
        public DbSet<ItemOption> ItemOptions { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationSelection> ApplicationSelections { get; set; }
        public DbSet<RiskCategory> RiskCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupInformation>()
                .HasMany(g => g.GroupItems)
                .WithOne(i => i.GroupInformation)
                .HasForeignKey(i => i.GroupInformationId);
        }
    }
}