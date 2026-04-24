using CLS.BackendAPI.Models.DTOs.Attendances;

namespace CLS.BackendAPI.Services
{
    public interface IAttendanceService
    {
        Task<IEnumerable<AttendanceRecordDto>> GetSessionRosterAsync(int sessionId);
        Task SubmitSessionAttendanceAsync(int sessionId, SubmitAttendanceRequest request);
    }
}
