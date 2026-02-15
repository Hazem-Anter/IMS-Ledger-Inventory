
using FluentValidation;

namespace IMS.Application.Features.Admin.Users.Queries.GetUser
{
    // This validator ensures that the GetUserQuery contains a valid user ID (greater than 0) before processing the query.
    public sealed class GetUserQueryValidator : AbstractValidator<GetUserQuery>
    {
        public GetUserQueryValidator()
        {

            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
