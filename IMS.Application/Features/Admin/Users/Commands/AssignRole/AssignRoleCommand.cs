using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Admin.Users.Commands.AssignRole
{
    // This command is used to assign a role to a user in the system.
    // It contains the user ID and the role to be assigned.
    public sealed record AssignRoleCommand(int UserId, string Role)
        : IRequest<Result<bool>>;
}
