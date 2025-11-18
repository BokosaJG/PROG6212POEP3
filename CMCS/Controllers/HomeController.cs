using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CMCS.Data;
using CMCS.Models;
using CMCS.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, IWebHostEnvironment environment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.UserRole = user.Role;
            ViewBag.UserName = user.FullName;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SubmitClaim()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user.Role != "Lecturer")
            {
                return Forbid();
            }

            var model = new ClaimViewModel
            {
                LecturerName = user.FullName,
                HourlyRate = user.HourlyRate
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitClaim(ClaimViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                var claim = new Claim
                {
                    LecturerId = user.Id,
                    LecturerName = user.FullName,
                    HoursWorked = model.HoursWorked,
                    HourlyRate = user.HourlyRate, // Use HR-set rate, not user input
                    AdditionalNotes = model.AdditionalNotes,
                    Status = "Pending",
                    SubmissionDate = DateTime.UtcNow
                };

                _context.Claims.Add(claim);
                await _context.SaveChangesAsync();

                // Handle file uploads
                if (model.SupportingDocuments != null && model.SupportingDocuments.Count > 0)
                {
                    await HandleFileUploads(model.SupportingDocuments, claim.Id);
                }

                TempData["SuccessMessage"] = "Claim submitted successfully!";
                return RedirectToAction("MyClaims");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MyClaims()
        {
            var user = await _userManager.GetUserAsync(User);
            var claims = await _context.Claims
                .Where(c => c.LecturerId == user.Id)
                .Include(c => c.Documents)
                .OrderByDescending(c => c.SubmissionDate)
                .ToListAsync();

            return View(claims);
        }

        [Authorize(Roles = "Coordinator,Manager")]
        [HttpGet]
        public async Task<IActionResult> PendingClaims()
        {
            var user = await _userManager.GetUserAsync(User);
            var pendingClaims = await _context.Claims
                .Where(c => c.Status == "Pending")
                .Include(c => c.Documents)
                .Include(c => c.Lecturer)
                .OrderBy(c => c.SubmissionDate)
                .ToListAsync();

            ViewBag.CurrentUserRole = user.Role;
            return View(pendingClaims);
        }

        [Authorize(Roles = "Coordinator,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveClaim(ClaimApprovalViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var claim = await _context.Claims
                .Include(c => c.Lecturer)
                .FirstOrDefaultAsync(c => c.Id == model.ClaimId);

            if (claim == null)
            {
                return NotFound();
            }

            claim.Status = "Approved";
            claim.ApprovalDate = DateTime.UtcNow;
            claim.ApprovedBy = user.FullName;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Claim approved successfully!";
            return RedirectToAction("PendingClaims");
        }

        [Authorize(Roles = "Coordinator,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectClaim(ClaimApprovalViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var claim = await _context.Claims
                .Include(c => c.Lecturer)
                .FirstOrDefaultAsync(c => c.Id == model.ClaimId);

            if (claim == null)
            {
                return NotFound();
            }

            claim.Status = "Rejected";
            claim.RejectionReason = model.RejectionReason;
            claim.ApprovalDate = DateTime.UtcNow;
            claim.ApprovedBy = user.FullName;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Claim rejected successfully!";
            return RedirectToAction("PendingClaims");
        }

        private async Task HandleFileUploads(IFormFileCollection files, int claimId)
        {
            var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads", "documents");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            foreach (var file in files)
            {
                if (file.Length > 0 && file.Length < 10 * 1024 * 1024) // 10MB limit
                {
                    var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx", ".jpg", ".png" };
                    var extension = Path.GetExtension(file.FileName).ToLower();

                    if (allowedExtensions.Contains(extension))
                    {
                        var fileName = Guid.NewGuid().ToString() + extension;
                        var filePath = Path.Combine(uploadsPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var document = new Document
                        {
                            ClaimId = claimId,
                            FileName = fileName,
                            OriginalFileName = file.FileName,
                            FilePath = $"/uploads/documents/{fileName}",
                            FileSize = file.Length,
                            ContentType = file.ContentType ?? "application/octet-stream"
                        };

                        _context.Documents.Add(document);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}