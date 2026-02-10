
using FluentValidation;

namespace IMS.Application.Features.Auth.Login
{
    // This class defines the validation rules for the LoginCommand using FluentValidation.
    public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(100);
        }
    }
}
