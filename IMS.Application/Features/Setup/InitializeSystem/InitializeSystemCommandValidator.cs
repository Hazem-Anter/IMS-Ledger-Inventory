

using FluentValidation;

namespace IMS.Application.Features.Setup.InitializeSystem
{
    // This validator is responsible for validating the InitializeSystemCommand,
    // which initializes the system by creating an initial admin user.
    public sealed class InitializeSystemCommandValidator : AbstractValidator<InitializeSystemCommand>
    {
        public InitializeSystemCommandValidator() 
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.UserName).NotEmpty().MinimumLength(3);

            // Basic rule; Identity will enforce complexity too.
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        }
    }
}
