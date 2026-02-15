
using IMS.Application.Abstractions.Auth;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Admin.Roles.Commands.CreateRole
{
    // This handler processes the CreateRoleCommand by calling the IRoleAdminService to create a new role based on the provided role name and returning the result wrapped in a Result object.
    public sealed class CreateRoleCommandHandler
        : IRequestHandler<CreateRoleCommand, Result<bool>>
    {
        private readonly IRoleAdminService _svc;
        public CreateRoleCommandHandler(IRoleAdminService svc)
        {
            _svc = svc;
        }

        public async Task<Result<bool>> Handle(
            CreateRoleCommand cmd,
            CancellationToken ct)
        {
            // 1) Call the service to create the role
            var role = await _svc.CreateRoleAsync(cmd.RoleName, ct);
            // 2) Return the result indicating success or failure
            return role
                ? Result<bool>.Ok(true)
                : Result<bool>.Fail("Failed to create role.");
        }
    }
}
