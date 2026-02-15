
using IMS.Application.Abstractions.Auth;
using IMS.Application.Common.Paging;
using IMS.Application.Common.Results;
using IMS.Application.Features.Admin.Users.Queries.GetUser;
using IMS.Application.Features.Admin.Users.Queries.ListUsers;
using IMS.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.Identity
{
    // This service class implements the IUserAdminService interface and provides methods for managing user accounts, such as activating/deactivating users,
    public sealed class UserAdminService : IUserAdminService
    {
        private readonly AuthDbContext _db;
        private readonly UserManager<ApplicationUser> _users;
        private readonly RoleManager<IdentityRole<int>> _roles;

        public UserAdminService(
            AuthDbContext db,
            UserManager<ApplicationUser> users,
            RoleManager<IdentityRole<int>> roles)
        {
            _db = db;
            _users = users;
            _roles = roles;
        }

        // This method activates a user by setting their lockout end date to null,
        // effectively unlocking the user account. It returns true if the operation is successful, and false otherwise.
        public async Task<bool> ActivateAsync(int userId, CancellationToken ct)
        {
            // 1) Find the user by their ID using the UserManager's FindByIdAsync method.
            // If the user is not found (i.e., null), return false, indicating that the activation failed.
            var user = await _users.FindByIdAsync(userId.ToString());
            if (user is null) return false;

            // 2) If the user is found,
            // call the SetLockoutEndDateAsync method of the UserManager to set the lockout end date to null.
            var r = await _users.SetLockoutEndDateAsync(user, null);

            // 3) Return true if the operation succeeded, and false otherwise.
            return r.Succeeded ? true : false;
        }

        // This method assigns a specified role to a user.
        // It performs several checks, such as verifying the existence of the user and the role,
        // and whether the user is already in the role. It returns true
        // if the role assignment is successful or if the user is already in the role, and false otherwise.
        public async Task<bool> AssignRoleAsync(int userId, string role, CancellationToken ct)
        {
            // 1) Trim the role string to remove any leading or trailing whitespace.
            role = role.Trim();

            // 2) Find the user by their ID using the UserManager's FindByIdAsync method.
            // If the user is not found (i.e., null), return false, indicating that the role assignment failed.
            var user = await _users.FindByIdAsync(userId.ToString());
            if (user is null) return false;

            // 3) Check if the specified role exists using the RoleManager's RoleExistsAsync method.
            // If the role does not exist, return false, indicating that the role assignment failed.
            if (!await _roles.RoleExistsAsync(role))
                return false;

            // 4) Check if the user is already in the specified role using the UserManager's IsInRoleAsync method.
            // If the user is already in the role, return true, indicating that the role assignment is considered successful since the user already has the role.
            var already = await _users.IsInRoleAsync(user, role);
            if (already) return true;

            // 5) If the user is not already in the role,
            // call the AddToRoleAsync method of the UserManager to add the user to the specified role.
            var r = await _users.AddToRoleAsync(user, role);

            // 6) Return true if the operation succeeded, and false otherwise.
            return r.Succeeded ? true : false;
        }

        // This method changes the password of a user. It first retrieves the user by their ID,
        // generates a password reset token, and then uses that token to reset the password to the new value provided.
        public async Task<bool> ChangePasswordAsync(int userId, string newPassword, CancellationToken ct)
        {
            // 1) Find the user by their ID using the UserManager's FindByIdAsync method.
            // If the user is not found (i.e., null), return false, indicating that the password change failed.
            var user = await _users.FindByIdAsync(userId.ToString());
            if (user is null) return false;

            // 2) If the user is found, generate a password reset token for the user using the UserManager's GeneratePasswordResetTokenAsync method.
            var token = await _users.GeneratePasswordResetTokenAsync(user);

            // 3) Use the generated token to reset the user's password to the new value provided by calling the ResetPasswordAsync method of the UserManager.
            var r = await _users.ResetPasswordAsync(user, token, newPassword);

            // 4) Return true if the operation succeeded, and false otherwise.
            return r.Succeeded ? true : false;
        }

        // This method deactivates a user by enabling lockout and setting the lockout end date to a far future date (DateTimeOffset.MaxValue).
        public async Task<bool> DeactivateAsync(int userId, CancellationToken ct)
        {
            // 1) Find the user by their ID using the UserManager's FindByIdAsync method.
            // If the user is not found (i.e., null), return false, indicating that the deactivation failed.
            var user = await _users.FindByIdAsync(userId.ToString());
            if (user is null) return false;

            // 2) If the user is found, check if the user is currently in the "Admin" role using the UserManager's IsInRoleAsync method.
            var isAdmin = await _users.IsInRoleAsync(user, "Admin");

            // If the user is in the "Admin" role, check if there is more than one user currently assigned to the "Admin" role.
            // If there is only one user in the "Admin" role, return false to prevent deactivating the last admin user.
            if (isAdmin)
            {
                var adminRole = await _roles.FindByNameAsync("Admin");
                var adminCount = await _db.UserRoles
                    .CountAsync(ur => ur.RoleId == adminRole.Id, ct);

                if (adminCount <= 1)
                    return false;
            }


            // 3) If the user is found, first enable lockout for the user by calling the SetLockoutEnabledAsync method of the UserManager with the value true.
            await _users.SetLockoutEnabledAsync(user, true);

            // 4) Then, set the lockout end date to a far future date (DateTimeOffset.MaxValue) by calling the SetLockoutEndDateAsync method of the UserManager.
            var r = await _users.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);

            // 5) Return true if the operation succeeded, and false otherwise.
            return r.Succeeded ? true : false;
        }

        // This method retrieves the details of a user by their ID.
        // It first finds the user using the UserManager.
        public async Task<UserDetailsDto> GetUserAsync(int id, CancellationToken ct)
        {
            // 1) Find the user by their ID using the UserManager's FindByIdAsync method.
            var user = await _users.FindByIdAsync(id.ToString());
            if (user is null) return null!;

            // 2) If the user is found, retrieve the list of roles assigned to the user using the UserManager's GetRolesAsync method.
            var roles = await _users.GetRolesAsync(user);

            // 3) Return a new UserDetailsDto containing the user's ID, email, username, lockout status,
            // lockout end date, and the list of roles (ordered alphabetically).
            return new UserDetailsDto
            (
                user.Id,
                user.Email ?? "",
                user.UserName ?? "",
                user.LockoutEnabled,
                user.LockoutEnd,
                roles.OrderBy(r => r).ToList()
            );
        }

        // This method retrieves a paginated list of users based on an optional search term. It allows filtering users by their email or username,
        // and returns a paged result containing user information such as ID, email, username, lockout status, and lockout end date.
        public async Task<PagedResult<UserListItemDto>> ListUsersAsync(
            string? search,
            int page,
            int pageSize,
            CancellationToken ct)
        {
            // 1) Ensure that the page number is at least 1 and the page size does not exceed 20.
            // This is done to prevent invalid pagination parameters.
            page = page < 1 ? 1 : page;
            pageSize = pageSize > 20 ? 20 : pageSize;

            // 2) Start a query on the Users DbSet of the AuthDbContext with AsNoTracking to improve performance since we are only reading data.
            var q = _db.Users.AsNoTracking();

            // 3) If a search term is provided (i.e., not null or whitespace),
            // trim the search term and convert it to lowercase for case-insensitive searching.
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                q = q.Where(u =>
                    (u.Email != null && u.Email.ToLower().Contains(search)) ||
                    (u.UserName != null && u.UserName.ToLower().Contains(search)));
            }

            // 4) Count the total number of users that match the search criteria using the CountAsync method.
            // This is necessary for pagination to know how many total items there are.
            var total = await q.CountAsync(ct);

            // 5) Retrieve a paginated list of users by ordering the query by user ID,
            // skipping the appropriate number of records based on the page number and page size,
            var items = await q
                .OrderBy(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserListItemDto(
                    u.Id,
                    u.Email ?? "",
                    u.UserName ?? "",
                    u.LockoutEnabled,
                    u.LockoutEnd))
                .ToListAsync(ct);

            // 6) Return a new PagedResult<UserListItemDto> containing the list of user items, total count, current page, and page size.
            return new PagedResult<UserListItemDto>(items, total, page, pageSize);
        }

        // This method removes a specified role from a user. It performs several checks,
        // such as verifying the existence of the user and the role,
        public async Task<bool> RemoveRoleAsync(int userId, string role, CancellationToken ct)
        {
            // 1) Trim the role string to remove any leading or trailing whitespace.
            // Find the user by their ID using the UserManager's FindByIdAsync method.
            // If the user is not found (i.e., null), return false, indicating that the role removal failed.
            role = role.Trim();
            var user = await _users.FindByIdAsync(userId.ToString());
            if (user is null) return false;

            // 2) Check if the specified role exists using the RoleManager's RoleExistsAsync method.
            if (!await _roles.RoleExistsAsync(role))
                return false;

            // 3) If the role to be removed is "Admin",
            // check if there is more than one user currently assigned to the "Admin" role.
            if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                var adminRole = await _roles.FindByNameAsync("Admin");
                if (adminRole is null)
                    return false;

                var adminCount = await _db.UserRoles
                    .CountAsync(ur => ur.RoleId == adminRole.Id, ct);

                if (adminCount <= 1)
                {
                    return false;
                }
            }

            // 4) Check if the user is currently in the specified role using the UserManager's IsInRoleAsync method.
            // If the user is not in the role, return true, indicating that the role removal is considered successful since the user does not have the role.
            var inRole = await _users.IsInRoleAsync(user, role);
            if (!inRole) return true;

            // 5) If the user is currently in the role, call the RemoveFromRoleAsync method of the UserManager to remove the user from the specified role.
            var r = await _users.RemoveFromRoleAsync(user, role);
            return r.Succeeded ? true : false;
        }
    }
}
