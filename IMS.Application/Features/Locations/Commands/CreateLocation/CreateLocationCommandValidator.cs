
using FluentValidation;

namespace IMS.Application.Features.Locations.Commands.CreateLocation
{
    // Validator for the CreateLocationCommand to ensure that the WarehouseId is greater than 0 and the Code is not empty and does not exceed 50 characters.
    public sealed class CreateLocationCommandValidator : AbstractValidator<CreateLocationCommand>
    {
        public CreateLocationCommandValidator()
        {
            RuleFor(x => x.WarehouseId).GreaterThan(0);
            RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
        }
    }
}
