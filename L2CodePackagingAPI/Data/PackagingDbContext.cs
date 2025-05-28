using L2CodePackagingAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace L2CodePackagingAPI.Data
{
    public class PackagingDbContext : DbContext
    {
        public PackagingDbContext(DbContextOptions<PackagingDbContext> options) : base(options)
        {
        }

        public DbSet<Box> Boxes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PackagingResult> PackagingResults { get; set; }
        public DbSet<PackagedProduct> PackagedProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Box Configuration
            modelBuilder.Entity<Box>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Height).IsRequired();
                entity.Property(e => e.Width).IsRequired();
                entity.Property(e => e.Length).IsRequired();
            });

            // Product Configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Height).IsRequired();
                entity.Property(e => e.Width).IsRequired();
                entity.Property(e => e.Length).IsRequired();
                entity.HasOne(p => p.Order)
                      .WithMany(o => o.Products)
                      .HasForeignKey(p => p.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Order Configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OrderNumber).HasMaxLength(100).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.HasIndex(e => e.OrderNumber).IsUnique();
            });

            // PackagingResult Configuration
            modelBuilder.Entity<PackagingResult>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.HasOne(pr => pr.Order)
                      .WithMany(o => o.PackagingResults)
                      .HasForeignKey(pr => pr.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(pr => pr.Box)
                      .WithMany()
                      .HasForeignKey(pr => pr.BoxId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // PackagedProduct Configuration
            modelBuilder.Entity<PackagedProduct>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(pp => pp.PackagingResult)
                      .WithMany(pr => pr.PackagedProducts)
                      .HasForeignKey(pp => pp.PackagingResultId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(pp => pp.Product)
                      .WithMany()
                      .HasForeignKey(pp => pp.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed Data
            modelBuilder.Entity<Box>().HasData(
                new Box { Id = 1, Name = "Caixa 1", Height = 30, Width = 40, Length = 80 },
                new Box { Id = 2, Name = "Caixa 2", Height = 80, Width = 50, Length = 40 },
                new Box { Id = 3, Name = "Caixa 3", Height = 50, Width = 80, Length = 60 }
            );
        }
    }
}
