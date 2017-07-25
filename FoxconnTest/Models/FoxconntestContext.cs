using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FoxconnTest.Models
{
    public partial class FoxconntestContext : DbContext
    {
        public virtual DbSet<Employees> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlite(@"data source=""FoxconnTest.db""");
        }

        // Method to create the Employee model based on the parameters retrieved by the SQLite database, using Microsoft Entity Framework.        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasKey(e => e.EmployeeId)
                    .HasName("sqlite_autoindex_Employees_1");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.EmailAddress).HasColumnName("email_address");

                entity.Property(e => e.EmployeeName)
                    .IsRequired()
                    .HasColumnName("employee_name");

                entity.Property(e => e.EmployeeSurname)
                    .IsRequired()
                    .HasColumnName("employee_surname");

                entity.Property(e => e.MobileNumber).HasColumnName("mobile_number");
            });
        }
    }
}