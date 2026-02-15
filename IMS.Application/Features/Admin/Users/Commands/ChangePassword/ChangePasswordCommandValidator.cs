
using FluentValidation;

namespace IMS.Application.Features.Admin.Users.Commands.ChangePassword
{
    // This validator ensures that the ChangePasswordCommand contains a valid user ID (greater than 0) and a new password that is not empty,
    // has a minimum length of 8 characters, and a maximum length of 100 characters before processing the command.
    public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0);

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(100);
        }
    }
}
