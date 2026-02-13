
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Warehouses.Queries.GetWarehouseById
{
    // Query for retrieving the details of a specific warehouse by its ID.
    public sealed record GetWarehouseByIdQuery(int id) : IRequest<Result<WarehouseDetailsDto>>;
}
