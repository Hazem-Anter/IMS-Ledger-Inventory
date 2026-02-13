
using FluentValidation;

namespace IMS.Application.Features.Locations.Commands.UpdateLocation
{
    // Validator for the UpdateLocationCommand.
    public sealed class UpdateLocationCommandValidator : AbstractValidator<UpdateLocationCommand>
    {
        public UpdateLocationCommandValidator()
        {
            RuleFor(x => x.WarehouseId).GreaterThan(0);
            RuleFor(x => x.LocationId).GreaterThan(0);
            RuleFor(x => x.Code).NotEmpty().MaximumLength(32);
        }
    }
}
