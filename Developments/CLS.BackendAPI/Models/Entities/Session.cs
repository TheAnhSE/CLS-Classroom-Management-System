using System;
using System.Collections.Generic;

namespace CLS.BackendAPI.Models.Entities;

public partial class Session
{
    public int SessionId { get; set; }

    public int SubjectId { get; set; }

    public int ClassroomId { get; set; }

    public int TeacherId { get; set; }

    public DateOnly SessionDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public string? Status { get; set; }

    public string? Notes { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedTime { get; set; }

    public DateTime? UpdatedTime { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual Classroom Classroom { get; set; } = null!;

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<SessionLearner> SessionLearners { get; set; } = new List<SessionLearner>();

    public virtual Subject Subject { get; set; } = null!;

    public virtual User Teacher { get; set; } = null!;
}
