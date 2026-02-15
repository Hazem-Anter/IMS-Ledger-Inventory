using IMS.Application.Abstractions.Auth;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Admin.Users.Commands.RemoveRole
{
    // This handler processes the RemoveRoleCommand by calling the IUserAdminService to remove a role from a user based on the provided user ID and role.
    public sealed class RemoveRoleCommandHandler 
        : IRequestHandler<RemoveRoleCommand, Result<bool>>
    {
        private readonly IUserAdminService _svc;

        public RemoveRoleCommandHandler(IUserAdminService svc)
        {
            _svc = svc;
        }

        public async Task<Result<bool>> Handle(
            RemoveRoleCommand cmd
            , CancellationToken ct)
        {
            // 1) Call the service to remove the role from the user
            var role = await _svc.RemoveRoleAsync(cmd.UserId, cmd.Role, ct);

            // 2) Return the result indicating success or failure
            return role
                ? Result<bool>.Ok(true)
                : Result<bool>.Fail("Failed to remove role from user.");
        }
    }
}
