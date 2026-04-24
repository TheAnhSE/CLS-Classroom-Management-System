using CLS.BackendAPI.Models.DTOs.Sessions;

namespace CLS.BackendAPI.Services
{
    public interface ISessionService
    {
        Task<SessionDto> CreateSessionAsync(CreateSessionRequest request);
        Task<SessionDto> UpdateSessionAsync(int id, UpdateSessionRequest request);
        Task<IEnumerable<SessionDto>> GetTimetableAsync(DateOnly? startDate, DateOnly? endDate, int? teacherId);
        // Task CancelSessionAsync(int id); // Optional based on UC-14, but keeping scope focused for now.
    }
}
