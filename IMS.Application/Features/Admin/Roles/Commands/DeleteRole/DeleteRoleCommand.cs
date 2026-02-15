
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Admin.Roles.Commands.DeleteRole
{
    // This command is used to delete an existing role from the system,
    // allowing administrators to remove roles that are no longer needed or relevant for access control and permissions management.
    public sealed record DeleteRoleCommand(
        string RoleName
        ) : IRequest<Result<bool>>;
}
