
using FluentValidation;

namespace IMS.Application.Features.Admin.Roles.Commands.DeleteRole
{
    // This validator ensures that the DeleteRoleCommand contains a valid role name that is not empty and has a maximum length of 50 characters before processing the command.
    public sealed class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleCommandValidator()
        {
            RuleFor(x => x.RoleName)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
