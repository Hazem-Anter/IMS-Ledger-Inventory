
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Setup.InitializeSystem
{
    // This command is responsible for initializing the system by creating an initial admin user.
    public sealed record InitializeSystemCommand(
        string Email,
        string Password,
        string UserName

     ) : IRequest<Result<bool>>;
}
