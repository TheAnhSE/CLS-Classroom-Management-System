using System;
using System.Collections.Generic;

namespace CLS.BackendAPI.Models.Entities;

public partial class Attendance
{
    public int AttendanceId { get; set; }

    public int SessionId { get; set; }

    public int LearnerId { get; set; }

    public string Status { get; set; } = null!;

    public int RecordedBy { get; set; }

    public DateTime? RecordedTime { get; set; }

    public string? Notes { get; set; }

    public virtual Learner Learner { get; set; } = null!;

    public virtual User RecordedByNavigation { get; set; } = null!;

    public virtual Session Session { get; set; } = null!;
}
