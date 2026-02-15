using IMS.Application.Abstractions.Auth;
using IMS.Application.Common.Paging;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Admin.Users.Queries.ListUsers
{
    // This handler processes the ListUsersQuery by calling the IUserAdminService to retrieve a paginated list of users based on the search criteria.
    public sealed class ListUsersQueryHandler
    : IRequestHandler<ListUsersQuery, Result<PagedResult<UserListItemDto>>>
    {
        private readonly IUserAdminService _svc;

        public ListUsersQueryHandler(IUserAdminService svc)
        {
            _svc = svc;
        }

        public async Task<Result<PagedResult<UserListItemDto>>> Handle(
            ListUsersQuery q, CancellationToken ct)
        {
            // 1) Call the service to get a paginated list of users based on the search criteria.
            var result = await _svc.ListUsersAsync(q.Search, q.Page, q.PageSize, ct);

            // 2) Check if the result is null. If it is, return a failure result with an appropriate error message.
            // Otherwise, return a success result containing the paginated list of users.
            return result is null 
                ? Result<PagedResult<UserListItemDto>>.Fail("Failed to retrieve users.")
                : Result<PagedResult<UserListItemDto>>.Ok(result);
        }
    }
}
