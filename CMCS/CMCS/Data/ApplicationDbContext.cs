using CMCS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Claim> Claims { get; set; }
        public DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure ApplicationUser table
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(u => u.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(u => u.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(u => u.Role)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(u => u.HourlyRate)
                    .HasColumnType("decimal(18,2)");

                entity.Property(u => u.IsActive)
                    .HasDefaultValue(true);
            });

            // Configure Claims table
            builder.Entity<Claim>(entity =>
            {
                entity.Property(c => c.LecturerName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(c => c.HoursWorked)
                    .HasColumnType("decimal(18,2)");

                entity.Property(c => c.HourlyRate)
                    .HasColumnType("decimal(18,2)");

                entity.Property(c => c.Status)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValue("Pending");

                entity.Property(c => c.RejectionReason)
                    .HasMaxLength(500);

                entity.HasOne(c => c.Lecturer)
                    .WithMany(u => u.Claims)
                    .HasForeignKey(c => c.LecturerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Documents table
            builder.Entity<Document>(entity =>
            {
                entity.Property(d => d.FileName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(d => d.OriginalFileName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(d => d.FilePath)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(d => d.ContentType)
                    .HasMaxLength(100);
            });

            // Seed initial data
            SeedData(builder);
        }

}