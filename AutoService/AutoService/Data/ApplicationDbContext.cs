using Microsoft.EntityFrameworkCore;
using AutoService.Models.Entities;

namespace AutoService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<CarType> CarTypes { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Comment> Comments { get; set; }
        // Статусы будем хранить как отдельную таблицу
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка связей
            modelBuilder.Entity<Request>()
                .HasOne(r => r.Car)
                .WithMany(c => c.Requests)
                .HasForeignKey(r => r.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.Client)
                .WithMany(u => u.Requests)
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.Master)
                .WithMany(u => u.MasterRequests)
                .HasForeignKey(r => r.MasterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Master)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.MasterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Request)
                .WithMany(r => r.Comments)
                .HasForeignKey(c => c.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь Car - CarType
            modelBuilder.Entity<Car>()
                .HasOne(c => c.CarType)
                .WithMany(ct => ct.Cars)  // Вот здесь используется Cars
                .HasForeignKey(c => c.CarTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь Car - Owner (если есть)
            modelBuilder.Entity<Car>()
                .HasOne(c => c.Owner)
                .WithMany()  // Если у User нет коллекции Cars
                .HasForeignKey(c => c.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Уникальные индексы
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();

            modelBuilder.Entity<Car>()
                .HasIndex(c => c.CarModel);

            // Настройка длины строк
            modelBuilder.Entity<User>()
                .Property(u => u.Type)
                .HasMaxLength(20);

            modelBuilder.Entity<Request>()
                .Property(r => r.RequestStatus)
                .HasMaxLength(50);
        }
    }
}