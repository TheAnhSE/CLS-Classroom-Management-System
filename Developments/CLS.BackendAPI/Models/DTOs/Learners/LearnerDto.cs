namespace CLS.BackendAPI.Models.DTOs.Learners
{
    public class LearnerDto
    {
        public int LearnerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public DateOnly EnrollmentDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public int? ParentId { get; set; }
        
        public ParentDto? Parent { get; set; }
    }
}
