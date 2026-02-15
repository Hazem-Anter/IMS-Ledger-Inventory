/// <summary>
/// 
/// LEGACY_DO_NOT_USE
/// 
/// </summary>
/// 

using IMS.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace IMS.Infrastructure.Persistence.Seeding
{
    public static class IdentitySeeder
    {
        /*
        private static readonly string[] Roles =
            ["Admin", "Manager", "Clerk", "Auditor"];

        public static async Task SeedAsync(
            RoleManager<IdentityRole<int>> roleManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration config)
        {
            // 1) Ensure roles
            foreach (var role in Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole<int>(role));
            }

            // 2) Ensure admin user
            var email = config["SeedAdmin:Email"];
            var password = config["SeedAdmin:Password"];

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return; // nothing to seed

            var admin = await userManager.FindByEmailAsync(email);
            if (admin is null)
            {
                admin = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(admin, password);
                if (!createResult.Succeeded)
                {
                    // Fail fast: seeding should be obvious when it breaks
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Admin user seed failed: {errors}");
                }
            }

            // 3) Ensure admin has Admin role
            if (!await userManager.IsInRoleAsync(admin, "Admin"))
                await userManager.AddToRoleAsync(admin, "Admin");
        }
        */
    }
}
