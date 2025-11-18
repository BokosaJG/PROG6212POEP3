using CMCS.Models; // Add this using directive

namespace CMCS.ViewModels
{
    public class ReportViewModel
    {
        public DateTime StartDate { get; set; } = DateTime.Now.AddMonths(-1);
        public DateTime EndDate { get; set; } = DateTime.Now;
        public string? Status { get; set; } // All, Approved, Pending, Rejected
        public string? LecturerId { get; set; }
    }

    public class ReportResultViewModel
    {
        public List<Claim> Claims { get; set; } = new List<Claim>();
        public decimal TotalAmount => Claims.Sum(c => c.TotalAmount);
        public int TotalClaims => Claims.Count;
        public DateTime GeneratedDate { get; set; } = DateTime.Now;
        public DateTime StartDate { get; internal set; }
        public DateTime EndDate { get; internal set; }
    }
}