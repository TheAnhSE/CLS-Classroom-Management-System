using System.ComponentModel.DataAnnotations;

namespace CLS.BackendAPI.Models.DTOs.Learners
{
    public class CreateLearnerRequest
    {
        // Learner Info
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        public DateOnly? DateOfBirth { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string Gender { get; set; } = string.Empty;

        public string? Notes { get; set; }

        // Parent Info
        [Required]
        [MaxLength(100)]
        public string ParentFullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string ParentEmail { get; set; } = string.Empty;

        [Required]
        [Phone]
        [MaxLength(20)]
        public string ParentPhoneNumber { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? ParentAddress { get; set; }

        [Required]
        [MaxLength(50)]
        public string RelationshipToLearner { get; set; } = string.Empty;
    }
}
