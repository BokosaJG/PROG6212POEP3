using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CMCS.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        // This is a computed property - it's read-only
        public string FullName => $"{FirstName} {LastName}";

        [Required]
        public string Role { get; set; } = string.Empty; // Lecturer, Coordinator, Manager, HR

        [Required]
        [Range(0, 1000, ErrorMessage = "Hourly rate must be between 0 and 1000")]
        public decimal HourlyRate { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<Claim> Claims { get; set; } = new List<Claim>();
    }
}