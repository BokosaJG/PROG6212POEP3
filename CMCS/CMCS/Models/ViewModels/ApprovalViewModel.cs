namespace CMCS.ViewModels
{
    public class ClaimApprovalViewModel
    {
        public int ClaimId { get; set; }
        public string Action { get; set; } = string.Empty; // Approve or Reject
        public string? RejectionReason { get; set; }
    }
}