
using FluentValidation;

namespace IMS.Application.Features.Warehouses.Commands.CreateWarehouse
{
    public sealed class CreateWarehouseCommandValidator : AbstractValidator<CreateWarehouseCommand>
    {
        public CreateWarehouseCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Code)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
