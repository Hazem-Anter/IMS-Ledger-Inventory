
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Admin.Roles.Commands.CreateRole
{
    // This command is used to create a new role in the system,
    // allowing administrators to define new roles that can be assigned to users for access control and permissions management.
    public sealed record CreateRoleCommand(
        string RoleName
        ) : IRequest<Result<bool>>;
}
