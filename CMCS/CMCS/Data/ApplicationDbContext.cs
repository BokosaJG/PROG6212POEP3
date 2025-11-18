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

        private void SeedData(ModelBuilder builder)
        {
            // Seed users
            var users = new[]
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "lecturer@cmcs.com",
                    NormalizedUserName = "LECTURER@CMCS.COM",
                    Email = "lecturer@cmcs.com",
                    NormalizedEmail = "LECTURER@CMCS.COM",
                    FirstName = "John",
                    LastName = "Lecturer",
                    Role = "Lecturer",
                    HourlyRate = 150,
                    EmailConfirmed = true,
                    PasswordHash = GetPasswordHash("Lecturer123!"),
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                new ApplicationUser
                {
                    Id = "2",
                    UserName = "coordinator@cmcs.com",
                    NormalizedUserName = "COORDINATOR@CMCS.COM",
                    Email = "coordinator@cmcs.com",
                    NormalizedEmail = "COORDINATOR@CMCS.COM",
                    FirstName = "Jane",
                    LastName = "Coordinator",
                    Role = "Coordinator",
                    HourlyRate = 200,
                    EmailConfirmed = true,
                    PasswordHash = GetPasswordHash("Coordinator123!"),
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                new ApplicationUser
                {
                    Id = "3",
                    UserName = "manager@cmcs.com",
                    NormalizedUserName = "MANAGER@CMCS.COM",
                    Email = "manager@cmcs.com",
                    NormalizedEmail = "MANAGER@CMCS.COM",
                    FirstName = "Bob",
                    LastName = "Manager",
                    Role = "Manager",
                    HourlyRate = 250,
                    EmailConfirmed = true,
                    PasswordHash = GetPasswordHash("Manager123!"),
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                new ApplicationUser
                {
                    Id = "4",
                    UserName = "hr@cmcs.com",
                    NormalizedUserName = "HR@CMCS.COM",
                    Email = "hr@cmcs.com",
                    NormalizedEmail = "HR@CMCS.COM",
                    FirstName = "HR",
                    LastName = "Administrator",
                    Role = "HR",
                    HourlyRate = 0,
                    EmailConfirmed = true,
                    PasswordHash = GetPasswordHash("HR123!"),
                    SecurityStamp = Guid.NewGuid().ToString()
                }
            };

            builder.Entity<ApplicationUser>().HasData(users);
        }

        private string GetPasswordHash(string password)
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            return passwordHasher.HashPassword(null, password);
        }
    }
}