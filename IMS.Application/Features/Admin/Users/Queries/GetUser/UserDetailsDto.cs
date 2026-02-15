
namespace IMS.Application.Features.Admin.Users.Queries.GetUser
{
    // This DTO represents the details of a user, including their roles, returned by the GetUserQuery.
    public sealed record UserDetailsDto(
        int Id,
        string Email,
        string UserName,
        bool LockoutEnabled,
        DateTimeOffset? LockoutEnd,
        IReadOnlyList<string> Roles);
}
