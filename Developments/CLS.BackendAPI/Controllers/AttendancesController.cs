using CLS.BackendAPI.Models.DTOs.Attendances;
using CLS.BackendAPI.Models.DTOs.Common;
using CLS.BackendAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CLS.BackendAPI.Controllers
{
    [Route("api/v1/sessions/{sessionId}/attendance")]
    [ApiController]
    [Authorize]
    public class AttendancesController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendancesController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSessionRoster(int sessionId)
        {
            var roster = await _attendanceService.GetSessionRosterAsync(sessionId);
            return Ok(ApiResponse<IEnumerable<AttendanceRecordDto>>.Success(roster));
        }

        [HttpPut]
        public async Task<IActionResult> SubmitAttendance(int sessionId, [FromBody] SubmitAttendanceRequest request)
        {
            await _attendanceService.SubmitSessionAttendanceAsync(sessionId, request);
            return Ok(ApiResponse<object>.Success(null, "Cập nhật điểm danh thành công")); // MSG-ATD-100
        }
    }
}
