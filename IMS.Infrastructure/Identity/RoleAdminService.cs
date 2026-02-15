
using IMS.Application.Abstractions.Auth;
using IMS.Application.Common.Results;
using IMS.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.Identity
{
    public sealed class RoleAdminService : IRoleAdminService
    {
        
        private static readonly HashSet<string> BuiltInRoles = new(StringComparer.OrdinalIgnoreCase)
        {
            "Admin", "Manager", "Clerk", "Auditor"
        };

        private readonly AuthDbContext _db;
        private readonly RoleManager<IdentityRole<int>> _roles;

        public RoleAdminService(AuthDbContext db, RoleManager<IdentityRole<int>> roles)
        {
            _db = db;
            _roles = roles;
        }

        // Creates a new role with the specified name. Returns true if the role was created successfully, false otherwise.
        public async Task<bool> CreateRoleAsync(string roleName, CancellationToken ct)
        {
            // 1) Trim the role name to remove any leading or trailing whitespace.
            roleName = roleName.Trim();

            // 2) Check if the role name is null, empty, or consists only of whitespace.
            // If it is, return false to indicate that the role cannot be created.
            if (string.IsNullOrWhiteSpace(roleName))
                return false;

            // 3) Check if a role with the same name already exists in the system.
            if (await _roles.RoleExistsAsync(roleName))
                return true;

            // 4) If the role name is valid and does not already exist, create a new role using the RoleManager and return true
            // if the creation was successful, or false if it failed.
            var r = await _roles.CreateAsync(new IdentityRole<int>(roleName));
            return r.Succeeded ? true : false;
        }

        // Deletes the role with the specified name. Returns true if the role was deleted successfully, false otherwise.
        public async Task<bool> DeleteRoleAsync(string roleName, CancellationToken ct)
        {
            // 1) Trim the role name to remove any leading or trailing whitespace.
            roleName = roleName.Trim();

            // 2) Check if the role name contains in the built-in roles list. If it does, return false to prevent deletion of built-in roles.
            if (BuiltInRoles.Contains(roleName))
                return false;

            // 3) Check if a role with the specified name exists in the system.
            // If it does not exist, return false to indicate that there is no role to delete. 
            var role = await _roles.FindByNameAsync(roleName);
            if (role is null) return false;

            // prevent delete if assigned to any user
            // 4) If the role exists, check if it is currently assigned to any users.
            // If it is assigned to any users, return false to prevent deletion of the role.
            var isAssigned = await _db.UserRoles.AnyAsync(ur => ur.RoleId == role.Id, ct);
            if (isAssigned) return false;

            // 5) If the role exists and is not assigned to any users,
            // delete the role using the RoleManager and return true
            var r = await _roles.DeleteAsync(role);
            return r.Succeeded ? true : false;
        }

        // Retrieves a list of all role names in the system, ordered alphabetically.
        public async Task<IReadOnlyList<string>> ListRolesAsync(CancellationToken ct)
        {
            var roles = await _roles.Roles
            .AsNoTracking()
            .OrderBy(r => r.Name)
            .Select(r => r.Name!)
            .ToListAsync(ct);

            return roles;
        }
    }
}
