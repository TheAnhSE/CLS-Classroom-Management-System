namespace CLS.BackendAPI.Models.DTOs.Dashboard
{
    public class DashboardStatsDto
    {
        public int TotalLearners { get; set; }
        public int ActivePackages { get; set; }
        public int SessionsToday { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
