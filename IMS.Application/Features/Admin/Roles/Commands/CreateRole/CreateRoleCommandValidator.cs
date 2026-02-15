
using FluentValidation;

namespace IMS.Application.Features.Admin.Roles.Commands.CreateRole
{
    // This validator ensures that the CreateRoleCommand contains a valid role name that is not empty,
    // has a maximum length of 50 characters,
    public sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(x => x.RoleName)
                .NotEmpty()
                .MaximumLength(50)
                .Matches("^[A-Za-z][A-Za-z0-9_\\- ]*$")
                .WithMessage("Role name must start with a letter and contain only letters, numbers, spaces, '_' or '-'.");
        }
    }
}
