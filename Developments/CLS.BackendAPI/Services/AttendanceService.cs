using CLS.BackendAPI.Exceptions;
using CLS.BackendAPI.Models;
using CLS.BackendAPI.Models.DTOs.Attendances;
using CLS.BackendAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CLS.BackendAPI.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ClsDbContext _context;

        public AttendanceService(ClsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AttendanceRecordDto>> GetSessionRosterAsync(int sessionId)
        {
            var session = await _context.Sessions
                .Include(s => s.SessionLearners)
                    .ThenInclude(sl => sl.Learner)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SessionId == sessionId);

            if (session == null)
            {
                throw new NotFoundException($"Buổi học (ID: {sessionId}) không tồn tại.");
            }

            var attendances = await _context.Attendances
                .Where(a => a.SessionId == sessionId)
                .AsNoTracking()
                .ToDictionaryAsync(a => a.LearnerId);

            var roster = session.SessionLearners.Select(sl =>
            {
                var hasAttendance = attendances.TryGetValue(sl.LearnerId, out var attendance);
                return new AttendanceRecordDto
                {
                    LearnerId = sl.LearnerId,
                    FirstName = sl.Learner.FirstName,
                    LastName = sl.Learner.LastName,
                    Status = hasAttendance ? attendance!.Status : null,
                    Notes = hasAttendance ? attendance!.Notes : null
                };
            }).ToList();

            return roster;
        }

        public async Task SubmitSessionAttendanceAsync(int sessionId, SubmitAttendanceRequest request)
        {
            var session = await _context.Sessions.FindAsync(sessionId);
            if (session == null)
            {
                throw new NotFoundException($"Buổi học (ID: {sessionId}) không tồn tại.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var existingAttendances = await _context.Attendances
                    .Where(a => a.SessionId == sessionId)
                    .ToDictionaryAsync(a => a.LearnerId);

                foreach (var item in request.Attendances)
                {
                    var learnerPackage = await _context.LearnerPackages
                        .Include(lp => lp.Package)
                        .Where(lp => lp.LearnerId == item.LearnerId &&
                                     lp.Package.SubjectId == session.SubjectId &&
                                     lp.Status == "Active")
                        .FirstOrDefaultAsync();

                    bool previouslyDeducted = false;
                    bool currentlyDeducted = IsDeductibleStatus(item.Status);

                    if (existingAttendances.TryGetValue(item.LearnerId, out var existing))
                    {
                        previouslyDeducted = IsDeductibleStatus(existing.Status);
                        
                        // Update existing record
                        existing.Status = item.Status;
                        existing.Notes = item.Notes;
                        existing.RecordedTime = DateTime.UtcNow;
                        existing.RecordedBy = 1; // System/Admin ID
                    }
                    else
                    {
                        // Create new record
                        var newAttendance = new Attendance
                        {
                            SessionId = sessionId,
                            LearnerId = item.LearnerId,
                            Status = item.Status,
                            Notes = item.Notes,
                            RecordedTime = DateTime.UtcNow,
                            RecordedBy = 1 // System/Admin ID
                        };
                        _context.Attendances.Add(newAttendance);
                    }

                    // Adjust LearnerPackage Balance if necessary
                    if (learnerPackage != null)
                    {
                        int delta = 0;
                        if (!previouslyDeducted && currentlyDeducted)
                        {
                            delta = -1; // Deduct
                        }
                        else if (previouslyDeducted && !currentlyDeducted)
                        {
                            delta = 1; // Refund
                        }

                        if (delta != 0)
                        {
                            learnerPackage.RemainingSessions += delta;
                            learnerPackage.UpdatedTime = DateTime.UtcNow;

                            // Mark as completed if 0
                            if (learnerPackage.RemainingSessions <= 0)
                            {
                                learnerPackage.RemainingSessions = 0;
                                learnerPackage.Status = "Completed";
                            }
                            else if (learnerPackage.RemainingSessions > 0 && learnerPackage.Status == "Completed")
                            {
                                learnerPackage.Status = "Active"; // Restore active status if refunded
                            }
                        }
                    }
                }

                // If all attendances are taken, maybe update Session status?
                // session.Status = "Completed";

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw; // Rethrow to let global middleware handle it (e.g. return 500)
            }
        }

        private static bool IsDeductibleStatus(string status)
        {
            // BR-ATD-03: Present or Unexcused Absent deduces session. Excused does not.
            return status == "Present" || status == "Absent";
        }
    }
}
