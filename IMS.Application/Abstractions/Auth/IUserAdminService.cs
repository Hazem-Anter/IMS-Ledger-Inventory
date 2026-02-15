
using IMS.Application.Common.Paging;
using IMS.Application.Common.Results;
using IMS.Application.Features.Admin.Users.Queries.GetUser;
using IMS.Application.Features.Admin.Users.Queries.ListUsers;

namespace IMS.Application.Abstractions.Auth
{
    // This service is responsible for managing users in the system.
    public interface IUserAdminService
    {
        Task<PagedResult<UserListItemDto>> ListUsersAsync(
        string? search,
        int page,
        int pageSize,
        CancellationToken ct);

        Task<UserDetailsDto> GetUserAsync(int id, CancellationToken ct);

        Task<bool> AssignRoleAsync(int userId, string role, CancellationToken ct);
        Task<bool> RemoveRoleAsync(int userId, string role, CancellationToken ct);

        Task<bool> ChangePasswordAsync(int userId, string newPassword, CancellationToken ct);

        Task<bool> DeactivateAsync(int userId, CancellationToken ct);
        Task<bool> ActivateAsync(int userId, CancellationToken ct);
    }
}
