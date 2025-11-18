using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMCS.Models
{
    public class Claim
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string LecturerId { get; set; } = string.Empty;

        [ForeignKey("LecturerId")]
        public virtual ApplicationUser Lecturer { get; set; } = null!;

        [Required]
        [Range(1, 180, ErrorMessage = "Hours worked must be between 1 and 180")]
        public decimal HoursWorked { get; set; }

        [Required]
        public decimal HourlyRate { get; set; }

        [NotMapped]
        public decimal TotalAmount => HoursWorked * HourlyRate;

        public string? AdditionalNotes { get; set; }

        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;

        public DateTime? ApprovalDate { get; set; }

        public string? ApprovedBy { get; set; }

        public string? RejectionReason { get; set; }

        // Navigation property for documents
        public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
        public string LecturerName { get; internal set; }
    }
}