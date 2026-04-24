using System.ComponentModel.DataAnnotations;

namespace CLS.BackendAPI.Models.DTOs.Learners
{
    public class UpdateLearnerRequest
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        public DateOnly? DateOfBirth { get; set; }
        
        [MaxLength(10)]
        public string? Gender { get; set; }

        public string? Notes { get; set; }
    }
}
