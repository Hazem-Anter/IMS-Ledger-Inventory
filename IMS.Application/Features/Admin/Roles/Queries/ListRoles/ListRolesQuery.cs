
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Admin.Roles.Queries.ListRoles
{
    public sealed record ListRolesQuery() : IRequest<Result<IReadOnlyList<string>>>;
}
