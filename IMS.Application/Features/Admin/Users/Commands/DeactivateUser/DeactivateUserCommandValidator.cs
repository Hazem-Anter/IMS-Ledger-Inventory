
using FluentValidation;

namespace IMS.Application.Features.Admin.Users.Commands.DeactivateUser
{
    // This validator ensures that the DeactivateUserCommand contains a valid user ID (greater than 0) before processing the command.
    public sealed class DeactivateUserCommandValidator : AbstractValidator<DeactivateUserCommand>
    {
        public DeactivateUserCommandValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
        }
    }
}
