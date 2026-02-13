
using FluentValidation;

namespace IMS.Application.Features.Locations.Commands.DeactivateLocation
{
    // Validator for the DeactivateLocationCommand to ensure that both WarehouseId and LocationId are greater than 0.
    public sealed class DeactivateLocationCommandValidator : AbstractValidator<DeactivateLocationCommand>
    {
        public DeactivateLocationCommandValidator()
        {
            RuleFor(x => x.WarehouseId).GreaterThan(0);
            RuleFor(x => x.LocationId).GreaterThan(0);
        }
    }
}
