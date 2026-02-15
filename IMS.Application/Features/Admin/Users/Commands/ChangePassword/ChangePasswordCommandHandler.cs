using IMS.Application.Abstractions.Auth;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Admin.Users.Commands.ChangePassword
{
    public sealed class ChangePasswordCommandHandler
        : IRequestHandler<ChangePasswordCommand, Result<bool>>
    {
        private readonly IUserAdminService _svc;
        public ChangePasswordCommandHandler(IUserAdminService svc)
        {
            _svc = svc;
        }

        public async Task<Result<bool>> Handle(
            ChangePasswordCommand cmd,
            CancellationToken ct)
        {
            // 1) Call the service to change the user's password
            var success = await _svc.ChangePasswordAsync(cmd.UserId, cmd.NewPassword, ct);
            // 2) Return the result indicating success or failure
            return success
                ? Result<bool>.Ok(true)
                : Result<bool>.Fail("Failed to change user's password.");
        }
    
    }
}
