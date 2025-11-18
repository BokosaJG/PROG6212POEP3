using System.ComponentModel.DataAnnotations;

namespace CMCS.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClaimId { get; set; }

        [Required]
        public string FileName { get; set; } = string.Empty;

        public string OriginalFileName { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public string ContentType { get; set; } = string.Empty;

        public DateTime UploadDate { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual Claim Claim { get; set; } = null!;
    }
}