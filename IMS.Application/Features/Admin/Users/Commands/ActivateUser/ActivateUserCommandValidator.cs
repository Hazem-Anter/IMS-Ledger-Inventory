
using FluentValidation;

namespace IMS.Application.Features.Admin.Users.Commands.ActivateUser
{
    // This validator ensures that the ActivateUserCommand contains a valid user ID (greater than 0) before processing the command.
    public sealed class ActivateUserCommandValidator : AbstractValidator<ActivateUserCommand>
    {
        public ActivateUserCommandValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
        }
    }
}
