
using FluentValidation;

namespace IMS.Application.Features.Admin.Users.Queries.ListUsers
{
    // This validator ensures that the ListUsersQuery contains valid pagination parameters (page number and page size)
    // and that the search string does not exceed a certain length before processing the query.
    public sealed class ListUsersQueryValidator : AbstractValidator<ListUsersQuery>
    {
        public ListUsersQueryValidator()
        {
            RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 200);

            RuleFor(x => x.Search)
                .MaximumLength(200)
                .When(x => x.Search is not null);
        }
    }
}
