using CLS.BackendAPI.Models.DTOs.Common;
using CLS.BackendAPI.Models.DTOs.Dashboard;
using CLS.BackendAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CLS.BackendAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var stats = await _dashboardService.GetSummaryStatsAsync();
            return Ok(ApiResponse<DashboardStatsDto>.Success(stats));
        }
    }
}
