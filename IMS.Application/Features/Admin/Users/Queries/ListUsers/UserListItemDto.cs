namespace IMS.Application.Features.Admin.Users.Queries.ListUsers
{
    // This DTO represents a user item in the list of users returned by the ListUsersQuery.
    public sealed record UserListItemDto(
    int Id,
    string Email,
    string UserName,
    bool LockoutEnabled,
    DateTimeOffset? LockoutEnd);
}
