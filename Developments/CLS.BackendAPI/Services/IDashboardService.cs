using CLS.BackendAPI.Models.DTOs.Dashboard;

namespace CLS.BackendAPI.Services
{
    public interface IDashboardService
    {
        Task<DashboardStatsDto> GetSummaryStatsAsync();
    }
}
