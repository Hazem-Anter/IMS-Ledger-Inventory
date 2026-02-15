using IMS.Application.Abstractions.Auth;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Admin.Users.Commands.DeactivateUser
{
    // This handler processes the DeactivateUserCommand by calling the IUserAdminService to deactivate a user based on the provided user ID.
    public sealed class DeactivateUserCommandHandler
        : IRequestHandler<DeactivateUserCommand, Result<bool>>
    {
        private readonly IUserAdminService _svc;
        public DeactivateUserCommandHandler(IUserAdminService svc)
        {
            _svc = svc;
        }

        public async Task<Result<bool>> Handle(
            DeactivateUserCommand cmd,
            CancellationToken ct)
        {
            // 1) Call the service to deactivate the user
            var success = await _svc.DeactivateAsync(cmd.UserId, ct);
            // 2) Return the result indicating success or failure
            return success
                ? Result<bool>.Ok(true)
                : Result<bool>.Fail("Failed to deactivate user.");
        }
    
    }
}
