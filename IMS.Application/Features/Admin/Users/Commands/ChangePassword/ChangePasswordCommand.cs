using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Admin.Users.Commands.ChangePassword
{
    // This command is used to change the password of a user in the system.
    public sealed record ChangePasswordCommand(
        int UserId,
        string NewPassword

        ) : IRequest<Result<bool>>;
}
