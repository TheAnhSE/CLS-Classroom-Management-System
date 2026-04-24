using CLS.BackendAPI.Exceptions;
using CLS.BackendAPI.Models;
using CLS.BackendAPI.Models.DTOs.Learners;
using CLS.BackendAPI.Models.DTOs.Sessions;
using CLS.BackendAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CLS.BackendAPI.Services
{
    public class SessionService : ISessionService
    {
        private readonly ClsDbContext _context;

        public SessionService(ClsDbContext context)
        {
            _context = context;
        }

        public async Task<SessionDto> CreateSessionAsync(CreateSessionRequest request)
        {
            // 1. Conflict Detection Algorithm (UC-12)
            await DetectConflictAsync(request.SessionDate, request.StartTime, request.EndTime, request.ClassroomId, request.TeacherId, null);

            // 2. Capacity Check (BR-SCH-03)
            var room = await _context.Classrooms.FindAsync(request.ClassroomId);
            if (room == null)
            {
                throw new ValidationException($"Phòng học (ID: {request.ClassroomId}) không tồn tại.");
            }

            if (request.LearnerIds.Count > room.Capacity)
            {
                throw new ConflictException("Sĩ số vượt quá sức chứa phòng học."); // MSG-SCH-411
            }

            // 3. Create Session
            var session = new Session
            {
                SubjectId = request.SubjectId,
                ClassroomId = request.ClassroomId,
                TeacherId = request.TeacherId,
                SessionDate = request.SessionDate,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Notes = request.Notes,
                Status = "Scheduled",
                CreatedBy = 1, // System or Admin ID
                CreatedTime = DateTime.UtcNow
            };

            // 4. Map Learners
            foreach (var learnerId in request.LearnerIds)
            {
                session.SessionLearners.Add(new SessionLearner
                {
                    LearnerId = learnerId
                });
            }

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            return await GetSessionDtoByIdAsync(session.SessionId);
        }

        public async Task<SessionDto> UpdateSessionAsync(int id, UpdateSessionRequest request)
        {
            var session = await _context.Sessions
                .Include(s => s.SessionLearners)
                .FirstOrDefaultAsync(s => s.SessionId == id);

            if (session == null)
            {
                throw new NotFoundException($"Buổi học (ID: {id}) không tồn tại.");
            }

            // 1. Conflict Detection Algorithm (Exclude current session)
            await DetectConflictAsync(request.SessionDate, request.StartTime, request.EndTime, request.ClassroomId, request.TeacherId, id);

            // 2. Capacity Check
            var room = await _context.Classrooms.FindAsync(request.ClassroomId);
            if (room != null && request.LearnerIds.Count > room.Capacity)
            {
                throw new ConflictException("Sĩ số vượt quá sức chứa phòng học."); // MSG-SCH-411
            }

            // 3. Update fields
            session.ClassroomId = request.ClassroomId;
            session.TeacherId = request.TeacherId;
            session.SessionDate = request.SessionDate;
            session.StartTime = request.StartTime;
            session.EndTime = request.EndTime;
            session.Notes = request.Notes;
            session.UpdatedTime = DateTime.UtcNow;

            // 4. Update Learners (Remove old, add new)
            _context.SessionLearners.RemoveRange(session.SessionLearners);
            
            foreach (var learnerId in request.LearnerIds)
            {
                session.SessionLearners.Add(new SessionLearner
                {
                    LearnerId = learnerId
                });
            }

            await _context.SaveChangesAsync();

            return await GetSessionDtoByIdAsync(session.SessionId);
        }

        public async Task<IEnumerable<SessionDto>> GetTimetableAsync(DateOnly? startDate, DateOnly? endDate, int? teacherId)
        {
            var query = _context.Sessions
                .Include(s => s.Subject)
                .Include(s => s.Classroom)
                .Include(s => s.Teacher)
                .AsNoTracking()
                .AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(s => s.SessionDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(s => s.SessionDate <= endDate.Value);
            }

            if (teacherId.HasValue)
            {
                query = query.Where(s => s.TeacherId == teacherId.Value);
            }

            var sessions = await query.ToListAsync();

            return sessions.Select(s => new SessionDto
            {
                SessionId = s.SessionId,
                SubjectId = s.SubjectId,
                SubjectName = s.Subject?.SubjectName ?? string.Empty,
                ClassroomId = s.ClassroomId,
                ClassroomName = s.Classroom?.RoomName ?? string.Empty,
                TeacherId = s.TeacherId,
                TeacherName = s.Teacher != null ? $"{s.Teacher.FirstName} {s.Teacher.LastName}" : string.Empty,
                SessionDate = s.SessionDate,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Status = s.Status ?? string.Empty,
                Notes = s.Notes
            });
        }

        private async Task DetectConflictAsync(DateOnly date, TimeOnly start, TimeOnly end, int roomId, int teacherId, int? excludeSessionId)
        {
            var overlappingSessions = await _context.Sessions
                .Where(s => s.SessionDate == date && 
                           (!excludeSessionId.HasValue || s.SessionId != excludeSessionId.Value) &&
                           s.StartTime < end && 
                           s.EndTime > start)
                .ToListAsync();

            if (overlappingSessions.Any(s => s.TeacherId == teacherId))
            {
                throw new ConflictException("Lỗi: Giáo viên đã có dải lịch trùng."); // MSG-SCH-409
            }

            if (overlappingSessions.Any(s => s.ClassroomId == roomId))
            {
                throw new ConflictException("Lỗi: Phòng học đang bận."); // MSG-SCH-410
            }
        }

        private async Task<SessionDto> GetSessionDtoByIdAsync(int sessionId)
        {
            var session = await _context.Sessions
                .Include(s => s.Subject)
                .Include(s => s.Classroom)
                .Include(s => s.Teacher)
                .Include(s => s.SessionLearners)
                    .ThenInclude(sl => sl.Learner)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SessionId == sessionId);

            if (session == null) return null!;

            return new SessionDto
            {
                SessionId = session.SessionId,
                SubjectId = session.SubjectId,
                SubjectName = session.Subject?.SubjectName ?? string.Empty,
                ClassroomId = session.ClassroomId,
                ClassroomName = session.Classroom?.RoomName ?? string.Empty,
                TeacherId = session.TeacherId,
                TeacherName = session.Teacher != null ? $"{session.Teacher.FirstName} {session.Teacher.LastName}" : string.Empty,
                SessionDate = session.SessionDate,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                Status = session.Status ?? string.Empty,
                Notes = session.Notes,
                EnrolledLearners = session.SessionLearners.Select(sl => new LearnerDto
                {
                    LearnerId = sl.Learner.LearnerId,
                    FirstName = sl.Learner.FirstName,
                    LastName = sl.Learner.LastName,
                    Status = sl.Learner.Status ?? string.Empty
                }).ToList()
            };
        }
    }
}
