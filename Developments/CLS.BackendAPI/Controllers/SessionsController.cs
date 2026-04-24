using CLS.BackendAPI.Models.DTOs.Common;
using CLS.BackendAPI.Models.DTOs.Sessions;
using CLS.BackendAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CLS.BackendAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class SessionsController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSession([FromBody] CreateSessionRequest request)
        {
            var session = await _sessionService.CreateSessionAsync(request);
            return Ok(ApiResponse<SessionDto>.Created(session, "Xếp lịch thành công")); // MSG-SCH-100
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSession(int id, [FromBody] UpdateSessionRequest request)
        {
            var session = await _sessionService.UpdateSessionAsync(id, request);
            return Ok(ApiResponse<SessionDto>.Success(session, "Cập nhật lịch thành công"));
        }

        [HttpGet("timetable")]
        public async Task<IActionResult> GetTimetable([FromQuery] DateOnly? startDate, [FromQuery] DateOnly? endDate, [FromQuery] int? teacherId)
        {
            var sessions = await _sessionService.GetTimetableAsync(startDate, endDate, teacherId);
            return Ok(ApiResponse<IEnumerable<SessionDto>>.Success(sessions));
        }
    }
}
