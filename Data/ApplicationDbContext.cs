using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Visitors.Models;

namespace Visitors.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Place> Places { get; set; }

        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Visitor>()
               .HasOne(v => v.User)
               .WithMany()
               .HasForeignKey(v => v.UserID);

            modelBuilder.Entity<Visitor>()
                .HasOne(v => v.Place)
                .WithMany()
                .HasForeignKey(v => v.PlaceId);
            modelBuilder.Entity<Visitor>()
                .HasOne(v => v.Category)
                .WithMany()
                .HasForeignKey(v => v.CategoryId);
            modelBuilder.Entity<ActivityLog>()
              .HasOne(c => c.User)
              .WithMany()
              .HasForeignKey(c => c.UserID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ErrorLog>()
              .HasOne(c => c.User)
              .WithMany()
              .HasForeignKey(c => c.UserID)
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
