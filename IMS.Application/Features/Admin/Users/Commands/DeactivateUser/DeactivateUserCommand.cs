using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Admin.Users.Commands.DeactivateUser
{
    // This command is used to deactivate a user in the system,
    // preventing them from accessing their account and performing any actions until they are reactivated.
    public sealed record DeactivateUserCommand(
        int UserId
        ) : IRequest<Result<bool>>;
}
