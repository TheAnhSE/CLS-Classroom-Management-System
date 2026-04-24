using CLS.BackendAPI.Models.DTOs.Learners;

namespace CLS.BackendAPI.Models.DTOs.Sessions
{
    public class SessionDto
    {
        public int SessionId { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        
        public int ClassroomId { get; set; }
        public string ClassroomName { get; set; } = string.Empty;
        
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        
        public DateOnly SessionDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }

        public List<LearnerDto> EnrolledLearners { get; set; } = new List<LearnerDto>();
    }
}
