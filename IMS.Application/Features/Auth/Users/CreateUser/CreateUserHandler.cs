
using IMS.Application.Abstractions.Auth;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Auth.Users.CreateUser
{
    // Handler for processing the CreateUserCommand,
    // which creates a new user with specified email, password, and roles.
    // It uses the IAuthService to perform the user creation and returns a Result containing the details of the newly created user.
    public sealed class CreateUserHandler 
        : IRequestHandler<CreateUserCommand, Result<CreateUserResponse>>
    {
        private readonly IAuthService _auth;
        public CreateUserHandler(IAuthService auth)
        {
            _auth = auth;
        }
        public async Task<Result<CreateUserResponse>> Handle(
            CreateUserCommand cmd,
            CancellationToken ct)
        {
            // 1) Validate the input command to ensure that email, password, and roles are provided. 
            if (cmd.Roles is null || cmd.Roles.Count == 0)
                return Result<CreateUserResponse>.Fail("At least one role is required.");

            // 2) Call the CreateUserAsync method of the IAuthService to create a new user.
            var created = await _auth.CreateUserAsync(cmd.Email, cmd.Password, cmd.Roles, ct);

            // 3) return a Result containing the details of the newly created user.
            return Result<CreateUserResponse>.Ok(
                new CreateUserResponse(created.UserId, created.Email, created.Roles));
        }
    
    }
}
