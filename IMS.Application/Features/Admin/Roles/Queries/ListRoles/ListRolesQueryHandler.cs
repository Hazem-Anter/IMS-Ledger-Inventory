
using IMS.Application.Abstractions.Auth;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Admin.Roles.Queries.ListRoles
{
    // This handler processes the ListRolesQuery by calling the IRoleAdminService to retrieve a list of roles and returning the result wrapped in a Result object.
    public sealed class ListRolesQueryHandler
        : IRequestHandler<ListRolesQuery, Result<IReadOnlyList<string>>>
    {
        private readonly IRoleAdminService _svc;
        public ListRolesQueryHandler(IRoleAdminService svc)
        {
            _svc = svc;
        }

        public async Task<Result<IReadOnlyList<string>>> Handle(
            ListRolesQuery q, CancellationToken ct)
        {
            // 1) Get the list of roles from the service
            var result = await _svc.ListRolesAsync(ct);

            // 2) Return the result, handling null case
            return result is null
                ? Result<IReadOnlyList<string>>.Fail("Failed to retrieve roles.")
                : Result<IReadOnlyList<string>>.Ok(result);
        }
    }
}
