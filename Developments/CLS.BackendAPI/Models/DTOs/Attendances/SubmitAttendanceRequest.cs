using System.ComponentModel.DataAnnotations;

namespace CLS.BackendAPI.Models.DTOs.Attendances
{
    public class StudentAttendanceRequest : IValidatableObject
    {
        [Required]
        public int LearnerId { get; set; }

        [Required]
        [RegularExpression("^(Present|Absent|Excused)$", ErrorMessage = "Status chỉ được phép: Present, Absent, Excused")]
        public string Status { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Status == "Excused" && string.IsNullOrWhiteSpace(Notes))
            {
                yield return new ValidationResult(
                    "Vắng có phép (Excused) bắt buộc phải ghi rõ lý do (Notes).",
                    new[] { nameof(Notes) });
            }
        }
    }

    public class SubmitAttendanceRequest
    {
        [Required]
        [MinLength(1, ErrorMessage = "Danh sách điểm danh không được trống.")]
        public List<StudentAttendanceRequest> Attendances { get; set; } = new List<StudentAttendanceRequest>();
    }
}
