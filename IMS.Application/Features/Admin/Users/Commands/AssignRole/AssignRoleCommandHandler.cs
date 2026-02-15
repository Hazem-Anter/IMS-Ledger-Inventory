using IMS.Application.Abstractions.Auth;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Admin.Users.Commands.AssignRole
{
    // This handler processes the AssignRoleCommand by calling the IUserAdminService to assign a role to a user based on the provided user ID and role.
    public sealed class AssignRoleCommandHandler 
        : IRequestHandler<AssignRoleCommand, Result<bool>>
    {
        private readonly IUserAdminService _svc;

        public AssignRoleCommandHandler(IUserAdminService svc)
        {
            _svc = svc;
        }

        public async Task<Result<bool>> Handle(
            AssignRoleCommand cmd, 
            CancellationToken ct)
        {
            // 1) Call the IUserAdminService to assign the role to the user.
            var role = await _svc.AssignRoleAsync(cmd.UserId, cmd.Role, ct);

            // 2) Return a Result<bool> indicating success or failure based on the outcome of the role assignment.
            return role == false
                ? Result<bool>.Fail("Failed to assign role.")
                : Result<bool>.Ok(true);
        }
    }
}
