using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Task.Models;

namespace Task;

public partial class ProductCustomerContext : DbContext
{
    public ProductCustomerContext()
    {
    }

    public ProductCustomerContext(DbContextOptions<ProductCustomerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<CustomerProduct> CustomerProducts { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server= localhost; Database= product_customer; Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerProduct>(entity =>
        {
            entity.ToTable("CustomerProduct");
        });
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Customer_1");

            entity.ToTable("Customer");

            entity.Property(e => e.Msisdn).HasMaxLength(11);
            entity.Property(e => e.Name).HasMaxLength(50);
        });


        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_User_1");

            entity.ToTable("User");

            entity.Property(e => e.Mail).HasMaxLength(70);
            entity.Property(e => e.Msisdn)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Roles).HasMaxLength(30);
            entity.Property(e => e.Token);
        });

        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
