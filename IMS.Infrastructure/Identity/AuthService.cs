
using IMS.Application.Abstractions.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.Identity
{
    // This class implements the IAuthService interface and provides methods for validating user credentials and retrieving user roles.
    public sealed class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _users;
        private readonly SignInManager<ApplicationUser> _signIn;
        private readonly RoleManager<IdentityRole<int>> _roles;

        public AuthService(
            UserManager<ApplicationUser> users,
            SignInManager<ApplicationUser> signIn,
            RoleManager<IdentityRole<int>> roles)
        {
            _users = users;
            _signIn = signIn;
            _roles = roles;
        }

        // This method validates the user's email and password.
        public async Task<AuthUser?> ValidateUserAsync(string email, string password, CancellationToken ct)
        {
            // 1) Find the user by email. If the user does not exist, return null.
            var user = await _users.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
            if (user is null) return null;

            // 2) Check if the provided password is correct. If the password is incorrect, return null.
            var ok = await _signIn.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
            if (!ok.Succeeded) return null;

            // 3) If the credentials are valid, return an AuthUser object containing the user's ID and email.
            return new AuthUser(user.Id, user.Email!);
        }

        // This method retrieves the roles associated with a user based on their user ID.
        public async Task<IReadOnlyList<string>> GetRolesAsync(int userId, CancellationToken ct)
        {
            // 1) Find the user by their ID. If the user does not exist, return an empty array.
            var user = await _users.FindByIdAsync(userId.ToString());
            if (user is null) return Array.Empty<string>();

            // 2) Retrieve the roles associated with the user and return them as a list of strings.
            var roles = await _users.GetRolesAsync(user);
            return roles.ToList();
        }

        // This method creates a new user with the specified email, password, and roles. 
        public async Task<CreateUserResult> CreateUserAsync(
            string email,
            string password,
            IReadOnlyList<string> roles,
            CancellationToken ct)
        {
            
            email = email.Trim().ToLowerInvariant();

            // 1) Check if a user with the provided email already exists. If such a user exists,
            // throw an exception to indicate that the email is already in use.
            var existing = await _users.FindByEmailAsync(email);
            if (existing is not null)
                throw new InvalidOperationException("User with this email already exists.");

            // 2) Verify that all specified roles exist in the system.
            // If any of the roles do not exist, throw an exception indicating which role is invalid.
            foreach (var role in roles)
            {
                if (!await _roles.RoleExistsAsync(role))
                    throw new InvalidOperationException($"Role '{role}' does not exist.");
            }

            // 3) Create a new user with the provided email and password.
            // If the user creation fails, throw an exception with the error details.
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var create = await _users.CreateAsync(user, password);
            if (!create.Succeeded)
            {
                var errors = string.Join(", ", create.Errors.Select(e => e.Description));
                throw new InvalidOperationException(errors);
            }

            // 4) Assign the specified roles to the newly created user.
            // If role assignment fails, throw an exception with the error details.
            var addRoles = await _users.AddToRolesAsync(user, roles);
            if (!addRoles.Succeeded)
            {
                var errors = string.Join(", ", addRoles.Errors.Select(e => e.Description));
                throw new InvalidOperationException(errors);
            }

            // 5) After successfully creating the user and assigning roles,
            // retrieve the final list of roles for the user
            // and return a CreateUserResult object containing the user's ID, email, and assigned roles.
            var finalRoles = (await _users.GetRolesAsync(user)).ToList();

            return new CreateUserResult(user.Id, user.Email!, finalRoles);
        }

    }
}
