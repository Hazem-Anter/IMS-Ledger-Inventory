
using IMS.Application.Abstractions.Read;
using IMS.Application.Common.Results;
using MediatR;

namespace IMS.Application.Features.Warehouses.Queries.GetWarehouseById
{
    // Handler for the GetWarehouseByIdQuery,
    // responsible for processing the query and returning the warehouse details.
    public sealed class GetWarehouseByIdQueryHandler
        : IRequestHandler<GetWarehouseByIdQuery, Result<WarehouseDetailsDto>>
    {
        private readonly IWarehouseReadService _read;

        public GetWarehouseByIdQueryHandler(IWarehouseReadService read)
        {
            _read = read;
        }

        public async Task<Result<WarehouseDetailsDto>> Handle(
            GetWarehouseByIdQuery q,
            CancellationToken ct)   
        {
            // 1) Use the read service to get the warehouse details by ID.
            var result = await _read.GetByIdAsync(q.id, ct);

            // 2) Check if the result is null. If it is, return a failure result with an appropriate message. 
            return result is null
                ? Result<WarehouseDetailsDto>.Fail("Warehouse not found.")
                : Result<WarehouseDetailsDto>.Ok(result);
        }
    }
}
