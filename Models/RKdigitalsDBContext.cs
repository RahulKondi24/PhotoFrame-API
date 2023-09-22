using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace RKdigitalsAPI.Models
{
    public partial class RKdigitalsDBContext : DbContext
    {
        public RKdigitalsDBContext()
        {
        }

        public RKdigitalsDBContext(DbContextOptions<RKdigitalsDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=KONDIVENKATESH\\SQLEXPRESS;Database=RKdigitalsDB;Trusted_Connection=True;");
            }
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("CART");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__CART__ProductId__6FE99F9F");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__CART__UserId__70DDC3D8");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("PRODUCTS");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Productdescription)
                    .IsUnicode(false)
                    .HasColumnName("PRODUCTDESCRIPTION");

                entity.Property(e => e.Productimage)
                    .IsUnicode(false)
                    .HasColumnName("PRODUCTIMAGE");

                entity.Property(e => e.Productname)
                    .IsUnicode(false)
                    .HasColumnName("PRODUCTNAME");

                entity.Property(e => e.Productprice).HasColumnName("PRODUCTPRICE");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("USERS");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.Fullname)
                    .IsUnicode(false)
                    .HasColumnName("FULLNAME");

                entity.Property(e => e.Mobilenumber).HasColumnName("MOBILENUMBER");

                entity.Property(e => e.Passwordhash).HasColumnName("PASSWORDHASH");

                entity.Property(e => e.Passwordkey).HasColumnName("PASSWORDKEY");

                entity.Property(e => e.Role)
                    .IsUnicode(false)
                    .HasColumnName("ROLE");

                entity.Property(e => e.Token)
                    .IsUnicode(false)
                    .HasColumnName("TOKEN");

                entity.Property(e => e.Username)
                    .IsUnicode(false)
                    .HasColumnName("USERNAME");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
