using FluentValidation;

namespace IMS.Application.Features.Locations.Commands.ActivateLocation
{
    // Validator for the ActivateLocationCommand to ensure that both WarehouseId and LocationId are greater than 0.
    public sealed class ActivateLocationCommandValidator : AbstractValidator<ActivateLocationCommand>
    {
        public ActivateLocationCommandValidator()
        {
            RuleFor(x => x.WarehouseId).GreaterThan(0);
            RuleFor(x => x.LocationId).GreaterThan(0);
        }
    }
}
