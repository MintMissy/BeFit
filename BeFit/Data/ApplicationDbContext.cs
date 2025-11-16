using BeFit.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<ExerciseType> ExerciseTypes { get; set; }
    public DbSet<TrainingSession> TrainingSessions { get; set; }
    public DbSet<ExerciseRecord> ExerciseRecords { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<TrainingSession>()
            .HasOne(ts => ts.User)
            .WithMany()
            .HasForeignKey(ts => ts.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ExerciseRecord>()
            .HasOne(er => er.TrainingSession)
            .WithMany(ts => ts.ExerciseRecords)
            .HasForeignKey(er => er.TrainingSessionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ExerciseRecord>()
            .HasOne(er => er.ExerciseType)
            .WithMany(et => et.ExerciseRecords)
            .HasForeignKey(er => er.ExerciseTypeId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<ExerciseRecord>()
            .Property(er => er.Weight)
            .HasPrecision(18, 2);
    }
}