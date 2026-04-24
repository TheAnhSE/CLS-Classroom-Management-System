using System;
using System.Collections.Generic;

namespace CLS.BackendAPI.Models.Entities;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int SessionId { get; set; }

    public int LearnerId { get; set; }

    public int TeacherId { get; set; }

    public int? PerformanceRating { get; set; }

    public string? BehavioralNotes { get; set; }

    public string? Recommendations { get; set; }

    public DateTime? SubmittedTime { get; set; }

    public DateTime SlaDeadline { get; set; }

    public bool IsOnTime { get; set; }

    public DateTime? CreatedTime { get; set; }

    public virtual Learner Learner { get; set; } = null!;

    public virtual Session Session { get; set; } = null!;

    public virtual User Teacher { get; set; } = null!;
}
