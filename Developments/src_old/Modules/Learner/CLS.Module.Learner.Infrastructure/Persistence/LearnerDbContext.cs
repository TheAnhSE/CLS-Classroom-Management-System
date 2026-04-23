using CLS.Module.Learner.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CLS.Module.Learner.Infrastructure.Persistence;

public class LearnerDbContext : DbContext
{
    public LearnerDbContext(DbContextOptions<LearnerDbContext> options) : base(options) { }

    public DbSet<Domain.Entities.Learner> Learners { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // EF Core mapping for Learner entity
        modelBuilder.Entity<Domain.Entities.Learner>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.ParentEmail).IsRequired().HasMaxLength(255);
            entity.Property(e => e.ParentPhone).HasMaxLength(50);
            entity.Property(e => e.Status).HasConversion<string>();
        });
    }
}
