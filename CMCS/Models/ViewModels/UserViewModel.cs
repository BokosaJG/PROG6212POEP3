using System.ComponentModel.DataAnnotations;

namespace CMCS.ViewModels
{
    public class UserViewModel
    {
        public string? Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;

        [Required]
        [Range(0, 1000, ErrorMessage = "Hourly rate must be between 0 and 1000")]
        [Display(Name = "Hourly Rate")]
        public decimal HourlyRate { get; set; }

        [Display(Name = "Temporary Password")]
        public string? TempPassword { get; set; }
    }
}