using System;
using System.Collections.Generic;

namespace CLS.BackendAPI.Models.Entities;

public partial class LearnerPackage
{
    public int LearnerPackageId { get; set; }

    public int LearnerId { get; set; }

    public int PackageId { get; set; }

    public DateOnly AssignedDate { get; set; }

    public DateOnly ExpiryDate { get; set; }

    public int TotalSessions { get; set; }

    public int RemainingSessions { get; set; }

    public string? Status { get; set; }

    public int AssignedBy { get; set; }

    public DateTime? CreatedTime { get; set; }

    public DateTime? UpdatedTime { get; set; }

    public virtual User AssignedByNavigation { get; set; } = null!;

    public virtual Learner Learner { get; set; } = null!;

    public virtual LearningPackage Package { get; set; } = null!;
}
