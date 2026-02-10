
using FluentValidation;

namespace IMS.Application.Features.Auth.Users.CreateUser
{
    // This class defines the validation rules for the CreateUserCommand using FluentValidation.
    public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private static readonly string[] AllowedRoles = { "Admin", "Manager", "Clerk", "Auditor" };

        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(100);

            RuleFor(x => x.Roles)
                .NotNull()
                .Must(r => r.Count > 0)
                .WithMessage("At least one role is required.");

            RuleForEach(x => x.Roles)
                .Must(r => AllowedRoles.Contains(r))
                .WithMessage($"Role must be one of: {string.Join(", ", AllowedRoles)}");
        }
    }
}
