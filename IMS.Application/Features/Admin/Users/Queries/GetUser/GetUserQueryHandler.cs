
using IMS.Application.Abstractions.Auth;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Admin.Users.Queries.GetUser
{
    // This handler processes the GetUserQuery to retrieve user details by ID.
    public sealed class GetUserQueryHandler
        : IRequestHandler<GetUserQuery, Result<UserDetailsDto>>
    {
        private readonly IUserAdminService _svc;

        public GetUserQueryHandler(IUserAdminService svc)
        {
            _svc = svc;
        }

        public async Task<Result<UserDetailsDto>> Handle(
            GetUserQuery q, CancellationToken ct)
        {
            // 1) Attempt to retrieve the user details using the provided ID.
            var user = await _svc.GetUserAsync(q.Id, ct);

            // 2) If the user is not found, return a failure result; otherwise, return the user details.
            return user is null
                ? Result<UserDetailsDto>.Fail("User not found.")
                : Result<UserDetailsDto>.Ok(user); 
        }
    }
}
