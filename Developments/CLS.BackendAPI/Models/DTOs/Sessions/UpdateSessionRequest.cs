using System.ComponentModel.DataAnnotations;

namespace CLS.BackendAPI.Models.DTOs.Sessions
{
    public class UpdateSessionRequest : IValidatableObject
    {
        [Required]
        public int ClassroomId { get; set; }

        [Required]
        public int TeacherId { get; set; }

        [Required]
        public DateOnly SessionDate { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        public string? Notes { get; set; }

        public List<int> LearnerIds { get; set; } = new List<int>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartTime >= EndTime)
            {
                yield return new ValidationResult(
                    "Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc.",
                    new[] { nameof(StartTime), nameof(EndTime) });
            }
        }
    }
}
