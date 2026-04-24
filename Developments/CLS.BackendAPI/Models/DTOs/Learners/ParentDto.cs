namespace CLS.BackendAPI.Models.DTOs.Learners
{
    public class ParentDto
    {
        public int ParentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Relationship { get; set; }
    }
}
