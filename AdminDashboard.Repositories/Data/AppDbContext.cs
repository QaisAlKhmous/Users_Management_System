using AdminDashboard.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminDashboard.Repositories.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("Data Source=localhost\\SQLEXPRESS;Initial Catalog=DB4;User ID=Qais;Password=Qais_2004;TrustServerCertificate=True") { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var user = modelBuilder.Entity<User>();

            user.Property(u => u.Username)
                .IsRequired();

            user.Property(u => u.Email)
                .IsRequired();

            user.Property(u => u.Password)
                .IsRequired();

            user.Property(u => u.Type)
                .IsRequired();

            user.Property(u => u.Username)
                .HasMaxLength(255)
                .IsRequired();


            user.Property(u => u.FirstName)
                .HasMaxLength(255)
                .IsRequired();

            user.Property(u => u.LastName)
                .HasMaxLength(255)
                .IsRequired();

            user.Property(u => u.IsActive)
              .IsRequired();

            user.Property(u => u.IsLocked)
              .IsRequired();

            user.Property(u => u.Status)
              .IsRequired();

            modelBuilder.Entity<User>()
                .HasOptional(u => u.CreatedByUser)
                .WithMany(u => u.CreatedUsers)
                .HasForeignKey(u => u.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasOptional(u => u.ApprovedByUser)
                .WithMany(u => u.ApprovedUsers)
                .HasForeignKey(u => u.ApprovedBy)
                .WillCascadeOnDelete(false);

            user.Property(u => u.ApprovedDate)
                 .IsOptional();

            base.OnModelCreating(modelBuilder);
        }

    }
}
