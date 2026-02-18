using IMS.Application.Common.Paging;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Admin.Users.Queries.ListUsers
{
    // This query is used to list users in the system with optional search and pagination.
    public sealed record ListUsersQuery(
        string? Search,
        int Page = 1,
        int PageSize = 10
        )
    : IRequest<Result<PagedResult<UserListItemDto>>>;
}
