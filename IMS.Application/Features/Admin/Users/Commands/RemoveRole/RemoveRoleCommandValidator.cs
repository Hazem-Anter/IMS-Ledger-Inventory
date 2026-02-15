
using FluentValidation;

namespace IMS.Application.Features.Admin.Users.Commands.RemoveRole
{
    // This validator ensures that the RemoveRoleCommand contains a valid user ID (greater than 0) and a valid role name
    public sealed class RemoveRoleCommandValidator : AbstractValidator<RemoveRoleCommand>
    {
        public RemoveRoleCommandValidator()
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
