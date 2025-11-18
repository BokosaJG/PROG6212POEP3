using Microsoft.EntityFrameworkCore;
using CMCS.Data;
using Microsoft.AspNetCore.Identity;
using CMCS.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity with custom user - DISABLE default UI
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders(); // Remove .AddDefaultUI() if you have it

builder.Services.AddRazorPages();

// Add Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireHR", policy => policy.RequireRole("HR"));
    options.AddPolicy("RequireLecturer", policy => policy.RequireRole("Lecturer"));
    options.AddPolicy("RequireCoordinator", policy => policy.RequireRole("Coordinator"));
    options.AddPolicy("RequireManager", policy => policy.RequireRole("Manager"));
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Coordinator", "Manager"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession(); // Add session middleware

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Remove this line if you have it: app.MapRazorPages();

// Initialize database and create default roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Ensure database is created with new schema
        await context.Database.EnsureCreatedAsync();

        // Create roles if they don't exist
        string[] roleNames = { "HR", "Lecturer", "Coordinator", "Manager" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Assign users to roles (users are already seeded in ApplicationDbContext)
        var users = await userManager.Users.ToListAsync();
        foreach (var user in users)
        {
            if (!await userManager.IsInRoleAsync(user, user.Role))
            {
                await userManager.AddToRoleAsync(user, user.Role);
            }
        }

        Console.WriteLine("Database created and seeded successfully.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

app.Run();