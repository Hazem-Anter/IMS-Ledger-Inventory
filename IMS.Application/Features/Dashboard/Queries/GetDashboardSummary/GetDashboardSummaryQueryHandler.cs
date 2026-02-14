
using IMS.Application.Abstractions.Read;
using IMS.Application.Common.Results;
using IMS.Application.Features.Dashboard.Dtos;
using MediatR;

namespace IMS.Application.Features.Dashboard.Queries.GetDashboardSummary
{
    // Handler for processing the GetDashboardSummaryQuery and returning the dashboard summary statistics
    public sealed class GetDashboardSummaryQueryHandler
    : IRequestHandler<GetDashboardSummaryQuery, Result<DashboardSummaryDto>>
    {
        private readonly IDashboardReadService _read;

        public GetDashboardSummaryQueryHandler(IDashboardReadService read)
        {
            _read = read;
        }

        public async Task<Result<DashboardSummaryDto>> Handle(
            GetDashboardSummaryQuery q,
            CancellationToken ct)
        {
            // 1) Retrieve the dashboard summary statistics using the read service
            var dto = await _read.GetSummaryAsync(ct);

            // 2) Return the summary data wrapped in a successful Result object
            return Result<DashboardSummaryDto>.Ok(dto);
        }
    }
}
