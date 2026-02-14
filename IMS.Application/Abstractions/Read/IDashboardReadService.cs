
using IMS.Application.Features.Dashboard.Dtos;

namespace IMS.Application.Abstractions.Read
{
    // Read service interface for retrieving dashboard summary statistics
    public interface IDashboardReadService
    {
        Task<DashboardSummaryDto> GetSummaryAsync(CancellationToken ct = default);
    }
}
