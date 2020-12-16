using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Entities
{
    public partial class ProductManagementContext : DbContext
    {
        public ProductManagementContext()
        {
        }

        public ProductManagementContext(DbContextOptions<ProductManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<ProductCategories> ProductCategories { get; set; }
        public virtual DbSet<Status> Status { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.;Initial Catalog=ProductManagement;TrustServerCertificate=True;Connection Timeout=30");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Products>(entity =>
            {
                entity.Property(e => e.AdditionalNote).HasMaxLength(255);

                entity.Property(e => e.Descriptions).HasMaxLength(255);

                entity.Property(e => e.Name)
                            .IsRequired()
                            .HasMaxLength(255);
            });

            modelBuilder.Entity<ProductCategories>(entity =>
            {
                entity.Property(e => e.Name)
                            .IsRequired()
                            .HasMaxLength(255);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(e => e.Name)
                            .IsRequired()
                            .HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
