using CLS.BackendAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CLS.BackendAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Learner> Learners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Learner>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                
                // Ensure email is unique
                entity.HasIndex(e => e.Email).IsUnique();
            });
        }
    }
}
