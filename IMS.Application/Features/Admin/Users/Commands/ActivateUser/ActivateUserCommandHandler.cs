using IMS.Application.Abstractions.Auth;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Admin.Users.Commands.ActivateUser
{
    // This handler processes the ActivateUserCommand by calling the IUserAdminService to activate a user based on the provided user ID.
    public sealed class ActivateUserCommandHandler
        : IRequestHandler<ActivateUserCommand, Result<bool>>
    {
        private readonly IUserAdminService _svc;
        public ActivateUserCommandHandler(IUserAdminService svc)
        {
            _svc = svc;
        }

        public async Task<Result<bool>> Handle(
            ActivateUserCommand cmd,
            CancellationToken ct)
        {
            // 1) Call the service to activate the user
            var success = await _svc.ActivateAsync(cmd.UserId, ct);
            // 2) Return the result indicating success or failure
            return success
                ? Result<bool>.Ok(true)
                : Result<bool>.Fail("Failed to activate user.");
        }
    
    }
}
