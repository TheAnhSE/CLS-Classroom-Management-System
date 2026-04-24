using CLS.BackendAPI.Models;
using CLS.BackendAPI.Models.DTOs.Dashboard;
using Microsoft.EntityFrameworkCore;

namespace CLS.BackendAPI.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ClsDbContext _context;

        public DashboardService(ClsDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardStatsDto> GetSummaryStatsAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var totalLearners = await _context.Learners.CountAsync();
            var activePackages = await _context.LearnerPackages.CountAsync(lp => lp.Status == "Active");
            var sessionsToday = await _context.Sessions.CountAsync(s => s.SessionDate == today);
            
            // Simple mock revenue calculation (sum of tuition fee of all active/completed LearnerPackages)
            // In a real app, you'd check Payments table. Here we use LearnerPackages joined with Packages.
            var totalRevenue = await _context.LearnerPackages
                .Include(lp => lp.Package)
                .SumAsync(lp => lp.Package.TuitionFee);

            return new DashboardStatsDto
            {
                TotalLearners = totalLearners,
                ActivePackages = activePackages,
                SessionsToday = sessionsToday,
                TotalRevenue = totalRevenue
            };
        }
    }
}
