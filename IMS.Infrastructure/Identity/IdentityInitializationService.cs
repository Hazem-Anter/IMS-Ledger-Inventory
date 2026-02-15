
using IMS.Application.Abstractions.Auth;
using IMS.Application.Common.Results;
using Microsoft.AspNetCore.Identity;

namespace IMS.Infrastructure.Identity
{
    // This class implements the IIdentityInitializationService interface and is responsible
    // for initializing the identity system by creating default roles and an initial admin user.
    public sealed class IdentityInitializationService : IIdentityInitializationService
    {
        private readonly UserManager<ApplicationUser> _user;
        private readonly RoleManager<IdentityRole<int>> _role;

        private static readonly string[] DefaultRoles = { "Admin", "Manager", "Clerk", "Auditor" };

        public IdentityInitializationService(
            UserManager<ApplicationUser> user,
            RoleManager<IdentityRole<int>> role)
        {
            _user = user;
            _role = role;
        }

        // This method initializes the identity system by creating default roles and an initial admin user.
        public async Task<Result<bool>> InitializeAsync(
            string email,
            string password,
            string userName,
            CancellationToken ct)
        {
            // 1) Check if there are any existing users in the system.
            // If there are, return a failure result indicating that the system is already initialized.
            if (_user.Users.Any())
                return Result<bool>.Fail("Syatem already initialized.");

            // 2) Create default roles if they do not already exist.
            foreach (var role in DefaultRoles)
            {
                if (!await _role.RoleExistsAsync(role))
                { 
                    var r = await _role.CreateAsync(new IdentityRole<int>(role));
                    if (!r.Succeeded)
                        return Result<bool>.Fail(string.Join(", ", r.Errors.Select(e => e.Description))); 
                }
            }

            // 3) Create an initial admin user with the provided email, password, and username.
            // Ensure that the email is confirmed for this user.
            var admin = new ApplicationUser
            {
                Email = email,
                UserName = userName,
                EmailConfirmed = true
            };

            var create = await _user.CreateAsync(admin, password);
            if (!create.Succeeded)
                return Result<bool>.Fail(string.Join(", ", create.Errors.Select(e => e.Description)));

            // 4) Assign the "Admin" role to the newly created admin user.
            var addRole = await _user.AddToRoleAsync(admin, "Admin");
            if (!addRole.Succeeded)
                return Result<bool>.Fail(string.Join(", ", addRole.Errors.Select(e => e.Description)));

            // 5) If all operations succeed, return a success result indicating that the identity system has been initialized successfully.
            return Result<bool>.Ok(true);
        }
    }
}
