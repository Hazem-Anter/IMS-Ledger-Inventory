 

namespace IMS.Application.Abstractions.Auth
{
    // This service is responsible for managing roles in the system.
    // It provides methods to list, create, and delete roles.
    public interface IRoleAdminService
    {
        Task<IReadOnlyList<string>> ListRolesAsync(CancellationToken ct);
        Task<bool> CreateRoleAsync(string roleName, CancellationToken ct);
        Task<bool> DeleteRoleAsync(string roleName, CancellationToken ct);
    }
}
