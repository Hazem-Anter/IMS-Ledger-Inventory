
using FluentValidation;

namespace IMS.Application.Features.Admin.Users.Commands.AssignRole
{
    // This validator ensures that the AssignRoleCommand contains a valid user ID (greater than 0) and a valid role name
    // (not empty, maximum length of 50 characters, and matches a specific pattern) before processing the command.
    public sealed class AssignRoleCommandValidator : AbstractValidator<AssignRoleCommand>
    {
        public AssignRoleCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0);

            RuleFor(x => x.Role)
                .NotEmpty()
                .MaximumLength(50)
                .Matches("^[A-Za-z][A-Za-z0-9_\\- ]*$")
                .WithMessage("Role name must start with a letter and contain only letters, numbers, spaces, '_' or '-'.");
        }
    }
}
