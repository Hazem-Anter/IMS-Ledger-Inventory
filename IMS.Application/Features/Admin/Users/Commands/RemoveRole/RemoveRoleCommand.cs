using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Admin.Users.Commands.RemoveRole
{
    // This command is used to remove a role from a user in the system.
    public sealed record RemoveRoleCommand(
        int UserId,
        string Role
        ) : IRequest<Result<bool>>;
}
