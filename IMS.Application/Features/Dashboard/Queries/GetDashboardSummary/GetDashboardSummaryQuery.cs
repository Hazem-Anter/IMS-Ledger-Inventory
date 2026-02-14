
using IMS.Application.Common.Results;
using IMS.Application.Features.Dashboard.Dtos;
using MediatR;

namespace IMS.Application.Features.Dashboard.Queries.GetDashboardSummary
{
    // Query to retrieve summary statistics for the dashboard
    public sealed record GetDashboardSummaryQuery()
        : IRequest<Result<DashboardSummaryDto>>;
}
