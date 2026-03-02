using Microsoft.EntityFrameworkCore;
using AutoService.Models;

namespace AutoService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Request>()
            .HasOne(r => r.Master)
            .WithMany(u => u.RequestsAsMaster)
            .HasForeignKey(r => r.MasterID)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Request>()
            .HasOne(r => r.Client)
            .WithMany(u => u.RequestsAsClient)
            .HasForeignKey(r => r.ClientID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Master)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.MasterID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Request)
            .WithMany(r => r.Comments)
            .HasForeignKey(c => c.RequestID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
