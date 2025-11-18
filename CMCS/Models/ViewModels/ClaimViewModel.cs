using System.ComponentModel.DataAnnotations;

namespace CMCS.ViewModels
{
    public class ClaimViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Lecturer name is required")]
        [Display(Name = "Lecturer Name")]
        public string LecturerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hours worked is required")]
        [Range(1, 200, ErrorMessage = "Hours worked must be between 1 and 200")]
        [Display(Name = "Hours Worked")]
        public decimal HoursWorked { get; set; }

        [Required(ErrorMessage = "Hourly rate is required")]
        [Range(0, 1000, ErrorMessage = "Hourly rate must be between 0 and 1000")]
        [Display(Name = "Hourly Rate")]
        public decimal HourlyRate { get; set; }

        [Display(Name = "Total Amount")]
        public decimal TotalAmount => HoursWorked * HourlyRate;

        [Display(Name = "Additional Notes")]
        public string? AdditionalNotes { get; set; }

        public string Status { get; set; } = "Pending";

        [Display(Name = "Submission Date")]
        public DateTime SubmissionDate { get; set; }

        public IFormFileCollection? SupportingDocuments { get; set; }
    }
}