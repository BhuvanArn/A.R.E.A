using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Action = System.Action;

namespace Database;

public class DatabaseContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Entities.Service> Services { get; set; }
    
    public DbSet<Entities.Action> Actions { get; set; }
    
    public DbSet<Reaction> Reactions { get; set; }
    
    public DbSet<PasswordReset> PasswordReset { get; set; }
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entities.Service>()
            .HasMany(s => s.Actions)
            .WithOne(a => a.Service)
            .HasForeignKey(a => a.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Entities.Service>()
            .HasMany(s => s.Reactions)
            .WithOne(r => r.Service)
            .HasForeignKey(r => r.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Entities.Service>()
            .HasOne<User>(s => s.User)
            .WithMany(u => u.Services)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Reaction>()
            .HasOne(r => r.Action)
            .WithMany(a => a.Reactions)
            .HasForeignKey(r => r.ActionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}