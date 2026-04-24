using System;
using System.Collections.Generic;

namespace CLS.BackendAPI.Models.Entities;

public partial class Learner
{
    public int LearnerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly? DateOfBirth { get; set; }

    public string Gender { get; set; } = null!;

    public int ParentId { get; set; }

    public DateOnly EnrollmentDate { get; set; }

    public string? Status { get; set; }

    public string? Notes { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedTime { get; set; }

    public DateTime? UpdatedTime { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<LearnerPackage> LearnerPackages { get; set; } = new List<LearnerPackage>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual Parent Parent { get; set; } = null!;

    public virtual ICollection<SessionLearner> SessionLearners { get; set; } = new List<SessionLearner>();
}
