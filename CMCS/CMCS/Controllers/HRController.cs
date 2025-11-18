using CMCS.Data;
using CMCS.Models;
using CMCS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Controllers
{
    [Authorize(Roles = "HR")]
    public class HRController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public HRController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = users.Select(u => new UserViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Role = u.Role,
                HourlyRate = u.HourlyRate
            }).ToList();

            return View(userViewModels);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tempPassword = GenerateTemporaryPassword();
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Role = model.Role,
                    HourlyRate = model.HourlyRate
                };

                var result = await _userManager.CreateAsync(user, tempPassword);
                if (result.Succeeded)
                {
                    model.TempPassword = tempPassword;
                    TempData["SuccessMessage"] = $"User created successfully! Temporary password: {tempPassword}";
                    return RedirectToAction("Users");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                HourlyRate = user.HourlyRate
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    return NotFound();
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.UserName = model.Email;
                user.Role = model.Role;
                user.HourlyRate = model.HourlyRate;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "User updated successfully!";
                    return RedirectToAction("Users");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var tempPassword = GenerateTemporaryPassword();
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, tempPassword);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"Password reset successfully! New temporary password: {tempPassword}";
            }
            else
            {
                TempData["ErrorMessage"] = "Error resetting password.";
            }

            return RedirectToAction("Users");
        }

        public async Task<IActionResult> Reports()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReport(ReportViewModel model)
        {
            // Fix date range - include the entire end date
            var startDate = model.StartDate.Date;
            var endDate = model.EndDate.Date.AddDays(1).AddSeconds(-1); // Include entire end date

            var query = _context.Claims
                .Include(c => c.Lecturer)
                .Include(c => c.Documents)
                .Where(c => c.SubmissionDate >= startDate && c.SubmissionDate <= endDate);

            if (!string.IsNullOrEmpty(model.Status) && model.Status != "All")
            {
                query = query.Where(c => c.Status == model.Status);
            }

            if (!string.IsNullOrEmpty(model.LecturerId))
            {
                query = query.Where(c => c.LecturerId == model.LecturerId);
            }

            var claims = await query.ToListAsync();

            // Debug information
            Console.WriteLine($"Date Range: {startDate} to {endDate}");
            Console.WriteLine($"Found {claims.Count} claims");

            var result = new ReportResultViewModel
            {
                Claims = claims,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                GeneratedDate = DateTime.Now
            };

            return View("ReportResult", result);
        }

        private string GenerateTemporaryPassword()
        {
            return "Temp123!"; // In production, generate a random secure password
        }


        [Authorize(Roles = "HR")]
        public async Task<IActionResult> DebugDatabase()
        {
            var users = await _context.Users.ToListAsync();
            var claims = await _context.Claims.Include(c => c.Lecturer).ToListAsync();

            ViewBag.Users = users;
            ViewBag.Claims = claims;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTestClaims()
        {
            var lecturer = await _userManager.Users.FirstOrDefaultAsync(u => u.Role == "Lecturer");

            if (lecturer != null)
            {
                var testClaims = new List<Claim>
        {
            new Claim
            {
                LecturerId = lecturer.Id,
                LecturerName = lecturer.FullName,
                HoursWorked = 40,
                HourlyRate = lecturer.HourlyRate,
                Status = "Approved",
                SubmissionDate = DateTime.Now.AddDays(-10),
                ApprovalDate = DateTime.Now.AddDays(-8),
                ApprovedBy = "Test Coordinator"
            },
            new Claim
            {
                LecturerId = lecturer.Id,
                LecturerName = lecturer.FullName,
                HoursWorked = 35,
                HourlyRate = lecturer.HourlyRate,
                Status = "Pending",
                SubmissionDate = DateTime.Now.AddDays(-5),
                AdditionalNotes = "Test pending claim"
            },
            new Claim
            {
                LecturerId = lecturer.Id,
                LecturerName = lecturer.FullName,
                HoursWorked = 45,
                HourlyRate = lecturer.HourlyRate,
                Status = "Rejected",
                SubmissionDate = DateTime.Now.AddDays(-15),
                ApprovalDate = DateTime.Now.AddDays(-12),
                ApprovedBy = "Test Manager",
                RejectionReason = "Test rejection reason"
            }
        };

                _context.Claims.AddRange(testClaims);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Test claims added successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "No lecturer found to create test claims.";
            }

            return RedirectToAction("Reports");
        }
    }

}