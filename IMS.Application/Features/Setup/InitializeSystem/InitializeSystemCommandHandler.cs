
using IMS.Application.Abstractions.Auth;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Setup.InitializeSystem
{
    // This command handler is responsible for handling the InitializeSystemCommand,
    // which initializes the system by creating an initial admin user.
    public sealed class InitializeSystemCommandHandler
        : IRequestHandler<InitializeSystemCommand, Result<bool>>
    {
        private readonly IIdentityInitializationService _init;

        public InitializeSystemCommandHandler(IIdentityInitializationService init)
        {
            _init = init;
        }


        public async Task<Result<bool>> Handle(
            InitializeSystemCommand cmd, 
            CancellationToken ct)
        {
            // 1) Use the IIdentityInitializationService to create an initial admin user with the provided email, password, and username.
            var user = await _init.InitializeAsync(cmd.Email, cmd.Password, cmd.UserName, ct);

            // 2) If the user creation fails (i.e., user is null),
            // return a failure result with an appropriate error message.  
            return user;
        }
    }
}
