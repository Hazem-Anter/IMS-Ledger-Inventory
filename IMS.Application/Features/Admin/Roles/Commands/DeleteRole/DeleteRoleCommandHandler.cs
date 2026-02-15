
using IMS.Application.Abstractions.Auth;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Admin.Roles.Commands.DeleteRole
{
    // This handler processes the DeleteRoleCommand by calling the IRoleAdminService to delete a role based on the provided role name and returning the result wrapped in a Result object.
    public sealed class DeleteRoleCommandHandler
        : IRequestHandler<DeleteRoleCommand, Result<bool>>
    {
        private readonly IRoleAdminService _svc;
        public DeleteRoleCommandHandler(IRoleAdminService svc)
        {
            _svc = svc;
        }

        public async Task<Result<bool>> Handle(
            DeleteRoleCommand q, CancellationToken ct)
        {
            // 1) Call the service to delete the role
            var success = await _svc.DeleteRoleAsync(q.RoleName, ct);
            // 2) Return the result
            return success
                ? Result<bool>.Ok(true)
                : Result<bool>.Fail($"Failed to delete role '{q.RoleName}'.");
        }
    
    }
}
