namespace CLS.BackendAPI.Models.DTOs.Attendances
{
    public class AttendanceRecordDto
    {
        public int LearnerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Status { get; set; }
        public string? Notes { get; set; }
    }
}
