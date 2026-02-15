
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Admin.Users.Queries.GetUser
{
    // This query is used to retrieve the details of a specific user by their ID, including their roles.
    public sealed record GetUserQuery(int Id) : IRequest<Result<UserDetailsDto>>;
}
