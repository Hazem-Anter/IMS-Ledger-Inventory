using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Admin.Users.Commands.ActivateUser
{
    // This command is used to activate a user in the system,
    // allowing them to access their account and perform actions after being deactivated.
    public sealed record ActivateUserCommand(
        int UserId
        ) : IRequest<Result<bool>>;

}
